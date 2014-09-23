import os, socket,json,struct,sys

# Bots = "None","Vaermina","Azura","MolagBal","Sanguine"
Side = {"Left":0,"Right":1,"Random":2}
Action = {"None":0,"Grip":1,"Release":2,"WaitForExit":3}

def GetHelloPackage():
	return {"LevelName":"Level1","Opponent":"None","Side":Side["Left"]}

def GetCommand(time, linearVelocity, angularVelocityGrad, action):
	return {"Action":action,"AngularVelocity":{"Grad":angularVelocityGrad},"LinearVelocity":linearVelocity,"Time":time}

def ReadAns(server):
	return json.loads(server.makefile('r').readline())

def Send(server, jsonObj):
	server.send(json.dumps(jsonObj))
	server.send('\n')
	return ReadAns(server)
	
def SendHelloPackage(server):
	server.send(json.dumps(GetHelloPackage()))
	server.send('\n')
	realSide = server.makefile('r').readline()
	return ReadAns(server)
	
def RunServer():
	if 'noRunServer' not in sys.argv:
		serverPath = "\\Cvarc\\build\\"
		os.chdir(serverPath)
		os.startfile(serverPath + "NetworkServer.exe.lnk")
	server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	server.connect(('localhost', 14000))
	return server

server = RunServer();
sensorData = SendHelloPackage(server)
sensorData = Send(server, GetCommand(1, 0, -90, Action["None"]))
sensorData = Send(server, GetCommand(1, 50, 0, Action["None"]))
sensorData = Send(server, GetCommand(1, 0, 0, Action["Grip"]))
sensorData = Send(server, GetCommand(1, -50, 0, Action["None"]))
sensorData = Send(server, GetCommand(1, 0, 90, Action["None"]))
sensorData = Send(server, GetCommand(1, 0, 0, Action["Release"]))
sensorData = Send(server, GetCommand(0, 0, 0, Action["WaitForExit"]))