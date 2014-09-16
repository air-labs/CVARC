import os, socket,json,struct

# Bots = "None","Vaermina","Azura","MolagBal","Sanguine"
Side = {"Left":0,"Right":1,"Random":2}
Action = {"None":0,"Grip":1,"Release":2,"WaitForExit":3}

def GetHelloPackage():
	return {"LevelName":"Level1","Opponent":"Azura","Side":Side["Left"]}

def GetCommand(time, linearVelocity, angularVelocityGrad, action):
	return {"Action":action,"AngularVelocity":{"Grad":angularVelocityGrad},"LinearVelocity":linearVelocity,"RobotId":0,"Time":time}
	
def Send(server, jsonObj):
	length = len(str(jsonObj))
	server.send(struct.pack('I', length))
	server.send(json.dumps(jsonObj))
	length = struct.unpack("<L", server.recv(4))[0]
	return json.loads(server.recv(length))
	
def RunServer():
	serverPath = "\\Cvarc\\CVARC\\NetworkServer\\bin\\Debug\\"
	os.chdir(serverPath)
	os.startfile(serverPath + "CVARC.Network.exe")
	server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	server.connect(('localhost', 14000))
	return server

server = RunServer();
sensorData = Send(server, GetHelloPackage())
sensorData = Send(server, GetCommand(1, 0, -90, Action["None"]))
sensorData = Send(server, GetCommand(1, 50, 0, Action["None"]))
sensorData = Send(server, GetCommand(1, 0, 0, Action["Grip"]))
sensorData = Send(server, GetCommand(1, -50, 0, Action["None"]))
sensorData = Send(server, GetCommand(1, 0, 90, Action["None"]))
sensorData = Send(server, GetCommand(1, 0, 0, Action["Release"]))
sensorData = Send(server, GetCommand(0, 0, 0, Action["WaitForExit"]))#WaitForExit