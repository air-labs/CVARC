package com.company;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.io.OutputStream;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.util.*;

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
    public boolean hasGrippedDetail;
    public Position positions[];

    public SensorData(JSONObject json) throws JSONException {
        robotId = json.getJSONObject("RobotId").getInt("Id");
        hasGrippedDetail = json.getJSONObject("DetailsInfo").getBoolean("HasGrippedDetail");
        JSONArray position = json.getJSONObject("Position").getJSONArray("PositionsData");
        positions = new Position[position.length()];
        for (int i = 0; i < position.length(); i++) {
            positions[i] = new Position(position.getJSONObject(i));
        }
    }

    @Override
    public String toString() {
        return "RobotID: " + robotId + "\nPosition 0: " + positions[0].toString() + "\nPosition 1: " + positions[1].toString() + "\nHasGrippedDetail: " + hasGrippedDetail + "\n";
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

    public static String readLine(Socket server) {
        int response;
        String string = "";
        try {
            while ((response = server.getInputStream().read()) != (int) '\n') {
                string += (char) response;
            }
        } catch (IOException e) {
            System.exit(-1);
        }
        return string;
    }

    public static void send(Socket server, JSONObject jsonObj) throws IOException {
        OutputStream outputStream = server.getOutputStream();
        outputStream.write(jsonObj.toString().getBytes("UTF-8"));
        outputStream.write("\n".getBytes("UTF-8"));
        outputStream.flush();
    }

    public static SensorData sendHelloPackage(Socket server, JSONObject jsonObj) throws Exception {
        send(server, jsonObj);
        readLine(server);
        return new SensorData(new JSONObject(readLine(server)));
    }

    public static SensorData sendPackage(Socket server, JSONObject jsonObj) throws Exception {
        send(server, jsonObj);
        return new SensorData(new JSONObject(readLine(server)));
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
        System.out.println(data);
        //System.out.println("X: " + data.positions[data.robotId].x + ", Y: " + data.positions[data.robotId].y);
    }

    public static void main(String[] args) throws Exception {
        Socket server = runServer(Arrays.asList(args).contains("noRunServer"));
        SensorData sensorData = sendHelloPackage(server, getHelloPackage());
        print(sensorData);
        sensorData = sendPackage(server, getCommand(1, 0, -90, action.get("None")));
        print(sensorData);
        sensorData = sendPackage(server, getCommand(1, 50, 0, action.get("None")));
        print(sensorData);
        sensorData = sendPackage(server, getCommand(1, 0, 0, action.get("Grip")));
        print(sensorData);
        sensorData = sendPackage(server, getCommand(1, -50, 0, action.get("None")));
        print(sensorData);
        sensorData = sendPackage(server, getCommand(1, 0, 90, action.get("None")));
        print(sensorData);
        sensorData = sendPackage(server, getCommand(1, 0, 0, action.get("Release")));
        print(sensorData);
        sensorData = sendPackage(server, getCommand(0, 0, 0, action.get("WaitForExit")));
        System.out.println(sensorData + " last");
    }
}
