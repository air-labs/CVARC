package com.company;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import sun.management.Sensor;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.util.*;

enum MapItemType {
    RedDetail,
    BlueDetail,
    GreenDetail,
    HorizontalWall,
    HorizontalBlueSocket,
    HorizontalGreenSocket,
    HorizontalRedSocket,
    VerticalRedSocket,
    VerticalWall,
    VerticalBlueSocket
}

class MapItem {
    public double x;
    public double y;
    public MapItemType tag;

    public MapItem(JSONObject json) throws JSONException {
        x = json.getDouble("X");
        y = json.getDouble("Y");
        tag = MapItemType.valueOf(json.getString("Tag"));
    }

    @Override
    public String toString() {
        return "X: " + x + " Y: " + y + " Tag: " + tag;
    }
}

class Position {
    public double angle;
    public double x;
    public double y;
    public int robotNumber;
    public Position(JSONObject json) throws JSONException {
        angle = json.getDouble("Angle");
        x = json.getDouble("X");
        y = json.getDouble("Y");
        robotNumber = json.getInt("RobotNumber");
    }

    @Override
    public String toString() {
        return "Angle: " + angle + " X: " + x + " Y: " + y;
    }
}

class SensorData {
    public int robotId;
    public Position positions[];
    public MapItem mapItems[] = {};

    public SensorData(JSONObject json) {
        try {
            robotId = json.getJSONObject("RobotId").getInt("Id");

            JSONArray position = json.getJSONObject("Position").getJSONArray("PositionsData");
            positions = new Position[position.length()];
            for (int i = 0; i < position.length(); i++) {
                positions[i] = new Position(position.getJSONObject(i));
            }

            JSONArray items = json.getJSONObject("MapSensor").getJSONArray("MapItems");
            mapItems = new MapItem[items.length()];
            for (int i = 0; i < items.length(); i++) {
                mapItems[i] = new MapItem(items.getJSONObject(i));
            }
        } catch (JSONException e) {
            System.out.println("Could not parse sensor data");
        }
    }

    @Override
    public String toString() {
        return "RobotID: " + robotId + "\n" + mapItems[0].toString() + "\n" + positions[0].toString();
    }
}

public class Client {
    public static final String bots[] = {"None", "Vaermina", "Azura", "MolagBal", "Sanguine"};
    public static final Map<String, Integer> side = new HashMap<String, Integer>() {{
        put("Left", 0);
        put("Right", 1);
        put("Random", 2);
    }};
    public static final Map<String, Integer> action = new HashMap<String, Integer>() {{
        put("None", 0);
        put("Grip", 1);
        put("Release", 2);
        put("WaitForExit", 3);
    }};

    public static JSONObject getHelloPackage() {
        return new JSONObject(new HashMap<String, Object>() {{
            put("LevelName", "Level1");
            put("Opponent", "None");
            put("Side", side.get("Left"));
        }});
    }

    public static JSONObject getCommand(final double time, final double linearVelocity, final double angularVelocityGrad, final int action) {
        return new JSONObject(new HashMap<String, Object>() {{
            put("Action", action);
            put("AngularVelocity", new HashMap<String, Object>() {{ put("Grad", angularVelocityGrad); }});
            put("LinearVelocity", linearVelocity);
            put("Time", time);
        }});
    }

    public static SensorData send(Socket server, JSONObject jsonObj) {
        try {
            server.getOutputStream().write(jsonObj.toString().getBytes("UTF-8"));
            server.getOutputStream().write("\n".getBytes("UTF-8"));
            int response;
            String string = "";
            while ((response = server.getInputStream().read()) != (int) '\n') {
                string += (char) response;
            }
            try {
                //System.out.println(string);
                return new SensorData(new JSONObject(string));
            } catch (JSONException e) {
                return new SensorData(new JSONObject());
            }
        } catch (IOException e) {
            System.out.println(e.getMessage());
            System.exit(1);
            return null;
        }
    }

    public static Socket runServer(boolean noRunServer) throws IOException {
        if (!noRunServer) {
            Runtime.getRuntime().exec("cmd /c start cmd.exe /K \"cd ../../build && NetworkServer.bat\"");
        }
        Socket server = new Socket();
        server.connect(new InetSocketAddress("localhost", 14000));
        return server;
    }

    public static void print(SensorData data) {
        System.out.println("X: " + data.positions[data.robotId].x + ", Y: " + data.positions[data.robotId].y);
    }

    public static void main(String[] args) throws Exception {
        boolean noRunServer = false;
        for (String arg : args) {
            if (arg.equals("noRunServer")) {
                noRunServer = true;
            }
        }

        Socket server = runServer(noRunServer);
        SensorData sensorData = send(server, getHelloPackage());

        sensorData = send(server, getCommand(1, 0, -90, action.get("None")));
        print(sensorData);
        sensorData = send(server, getCommand(1, 50, 0, action.get("None")));
        print(sensorData);
        sensorData = send(server, getCommand(1, 0, 0, action.get("Grip")));
        print(sensorData);
        sensorData = send(server, getCommand(1, -50, 0, action.get("None")));
        print(sensorData);
        sensorData = send(server, getCommand(1, 0, 90, action.get("None")));
        print(sensorData);
        sensorData = send(server, getCommand(1, 0, 0, action.get("Release")));
        print(sensorData);
        sensorData = send(server, getCommand(0, 0, 0, action.get("WaitForExit")));
        print(sensorData);
    }
}
