using System;
using AIRLab.Mathematics;
using CVARC.Basic.Controllers;
using CVARC.Network;
using ClientBase;
using RepairTheStarship.Sensors;

namespace Client
{
	internal class Program
	{
		private static readonly ClientSettings Settings = new ClientSettings
		{
			Side = Side.Left, //Переключив это поле, можно отладить алгоритм для левой или правой стороны, а также для произвольной стороны, назначенной сервером
			LevelName = LevelName.Level1, //Задается уровень, в котором вы хотите принять участие
			MapNumber=-1 //Задавая различные значения этого поля, вы можете сгенерировать различные случайные карты
		};

		//Это не нужно менять
		const string NetworkServerDirectory = ".\\";

		private static void Main(string[] args)
		{
			var server = new CvarcClient(args, Settings, NetworkServerDirectory).GetServer<PositionSensorsData>();
			var helloPackageAns = server.Run();

			//Здесь вы можете узнать сторону, назначенную вам сервером в случае, если запросили Side.Random. 
			//ВАЖНО!
			//Side и MapNumber влияют на сервер только на этапе отладки. В боевом режиме и то, и другое будет назначено сервером
			//вне зависимости от того, что вы указали в Settings! Поэтому ваш итоговый алгоритм должен использовать helloPackageAns.RealSide
			Console.WriteLine("Your Side: {0}", helloPackageAns.RealSide); 

			PositionSensorsData sensorsData = null;

			//Так вы можете отправлять различные команды. По результатам выполнения каждой команды, вы получите sensorsData, 
			//который содержит информацию о происходящем на поле
			sensorsData = server.SendCommand(new Command { AngularVelocity = Angle.FromGrad(-90), Time = 1 });
			sensorsData = server.SendCommand(new Command { LinearVelocity = 50, Time = 1 });
			sensorsData = server.SendCommand(new Command { Action = CommandAction.Grip, Time = 1 });
			sensorsData = server.SendCommand(new Command { LinearVelocity = -50, Time = 1 });
			sensorsData = server.SendCommand(new Command { AngularVelocity = Angle.FromGrad(90), Time = 1 });
			sensorsData = server.SendCommand(new Command { Action = CommandAction.Release, Time = 1 });

			//Используйте эту команду в конце кода для того, чтобы в режиме отладки все окна быстро закрылись, когда вы откатали алгоритм.
			//Если вы забудете это сделать, сервер какое-то время будет ожидать команд от вашего отвалившегося клиента. 
			server.Exit();
		}
	}
}
