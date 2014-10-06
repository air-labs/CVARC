import os
import socket
import json
import sys


# Bots = "None", "Vaermina", "Azura", "MolagBal", "Sanguine"
Side = {"Left": 0, "Right": 1, "Random": 2}
Action = {"None": 0, "Grip": 1, "Release": 2, "WaitForExit": 3}


class MapItem:
    def __init__(self, json_obj):
        self.x = json_obj["X"]
        self.y = json_obj["Y"]
        self.tag = json_obj["Tag"]
    
    def __str__(self):
        return "X: {}, Y: {}, Tag: {}.".format(self.x, self.y, self.tag)


class Position:
    def __init__(self, json_obj):
        self.angle = json_obj["Angle"]
        self.x = json_obj["X"]
        self.y = json_obj["Y"]
        self.robot_number = json_obj["RobotNumber"]
        
    def __str__(self):
        return "RobotNumber: {}, X: {}, Y: {}, Angle: {}.".format(self.robot_number, 
                                                                  self.x, 
                                                                  self.y, 
                                                                  self.angle)


class SensorData:
    def __init__(self, json_obj):
        try:
            self.robot_id = json_obj["RobotId"]["Id"]
            self.positions = []
            self.map_items = []
            position = json_obj["Position"]["PositionsData"]
            for i in range(len(position)):
                self.positions.append(Position(position[i]))
            
            items = json_obj["MapSensor"]["MapItems"]
            for i in range(len(items)):
                self.map_items.append(MapItem(items[i]))
        except KeyError:
            print("Incorrect sensor data")


def get_hello_package():
    return {"LevelName": "Level1", "Opponent": "None", "Side": Side["Left"]}


def get_command(time, linear_velocity, angular_velocity_grad, action):
    return {"Action": action,
            "AngularVelocity": {"Grad": angular_velocity_grad},
            "LinearVelocity": linear_velocity,
            "Time": time}


def read_ans(server):
    return json.loads(server.makefile('r').readline())


def sendUtf8(server, string):
	server.send(string.encode('utf-8'))
	
def send(server, json_obj):
	sendUtf8(server, json.dumps(json_obj))
	sendUtf8(server, '\n')
	return read_ans(server)

def send_hello_package(server):
    sendUtf8(server, json.dumps(get_hello_package()))
    sendUtf8(server, '\n')
    real_side = server.makefile('r').readline()
    return read_ans(server)

    
def run_server():
    if 'noRunServer' not in sys.argv:
        os.chdir("../../build")
        os.startfile("NetworkServer.bat")
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.connect(('localhost', 14000))
    return server


def print_sens_data(data):
    print("X: {}, Y: {}".format(data.positions[data.robot_id].x, data.positions[data.robot_id].y))


def main():
    server = run_server()
    sensor_data = SensorData(send_hello_package(server))
    sensor_data = SensorData(send(server, get_command(1, 0, -90, Action["None"])))
    print_sens_data(sensor_data)
    sensor_data = SensorData(send(server, get_command(1, 50, 0, Action["None"])))
    print_sens_data(sensor_data)
    sensor_data = SensorData(send(server, get_command(1, 0, 0, Action["Grip"])))
    print_sens_data(sensor_data)
    sensor_data = SensorData(send(server, get_command(1, -50, 0, Action["None"])))
    print_sens_data(sensor_data)
    sensor_data = SensorData(send(server, get_command(1, 0, 90, Action["None"])))
    print_sens_data(sensor_data)
    sensor_data = SensorData(send(server, get_command(1, 0, 0, Action["Release"])))
    print_sens_data(sensor_data)
    sensor_data = SensorData(send(server, get_command(0, 0, 0, Action["WaitForExit"])))
    print_sens_data(sensor_data)

    
if __name__ == '__main__':
    main()