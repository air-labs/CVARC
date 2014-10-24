import os,socket,json,sys

Side = {"Left": 0, "Right": 1, "Random": 2}
Action = {"None": 0, "Grip": 1, "Release": 2, "WaitForExit": 3}

class Position:
    def __init__(self, x, y, angle):
        self.angle = angle
        self.x = x
        self.y = y
    def __str__(self):
        return "X: {}, Y: {}, Angle: {}.".format(self.x, self.y, self.angle)

class JsonSensorData:
    def __init__(self, json_obj):
        self.json_obj = json_obj;

    def get_robot_id(self):
        return self.json_obj["RobotId"]["Id"]

    def has_gripped_detail(self):
        return self.json_obj["DetailsInfo"]["HasGrippedDetail"]

    def get_positions(self):
        positions = [];
        for i in self.json_obj["Position"]["PositionsData"]:
            positions.append(Position(i["X"], i["Y"], i["Angle"]))
        return positions

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
    print "RealSide: ", real_side
    return read_ans(server)
    
def run_server():
    if 'noRunServer' not in sys.argv:
        os.chdir("../../build")
        os.startfile("NetworkServer.bat")
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.connect(('localhost', 14000))
    return server

def main():
    server = run_server()
    sensor_data = JsonSensorData(send_hello_package(server))
    print "Robot Id: ", sensor_data.get_robot_id()
    print "My robot position: ", sensor_data.get_positions()[sensor_data.get_robot_id()]
    print "Has gripped detail: ", sensor_data.has_gripped_detail()
    sensor_data = JsonSensorData(send(server, get_command(1, 0, -90, Action["None"])))
    sensor_data = JsonSensorData(send(server, get_command(1, 50, 0, Action["None"])))
    sensor_data = JsonSensorData(send(server, get_command(1, 0, 0, Action["Grip"])))
    sensor_data = JsonSensorData(send(server, get_command(1, -50, 0, Action["None"])))
    sensor_data = JsonSensorData(send(server, get_command(1, 0, 90, Action["None"])))
    sensor_data = JsonSensorData(send(server, get_command(1, 0, 0, Action["Release"])))
    sensor_data = JsonSensorData(send(server, get_command(0, 0, 0, Action["WaitForExit"])))

if __name__ == '__main__':
    main()