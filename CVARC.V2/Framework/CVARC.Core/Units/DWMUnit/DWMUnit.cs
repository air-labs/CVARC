using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
namespace CVARC.V2
{
	public class DWMUnit : IUnit
	{
		const double TriggerFrequency = 0.005;
		DifWheelMovement movement;
		IDWMRobot actor;
		IDWMRules rules;

		public DWMUnit(IActor actor)
		{
			this.actor = Compatibility.Check<IDWMRobot>(this,actor);
			actor.World.Clocks.AddTrigger(new TimerTrigger(UpdateSpeed, TriggerFrequency));
			rules = Compatibility.Check<IDWMRules>(this, actor.Rules);
		}

		public UnitResponse ProcessCommand(object _command)
		{ 
			var command = Compatibility.Check<IDWMCommand>(this,_command);
			if (command.DifWheelMovement == null) return UnitResponse.Denied();
			this.movement = command.DifWheelMovement;
			return UnitResponse.Accepted(command.DifWheelMovement.Duration);
		}
        /// <summary>
        /// Update unit speed based on wheels speed
        /// </summary>
        /// <param name="currentTime"></param>
		public void UpdateSpeed(double currentTime)
		{
			if (movement == null) return;

            var requestedAngular = rules.WheelRadius * (movement.RightRotatingVelocity.Radian - movement.LeftRotatingVelocity.Radian) / rules.DistanceBetweenWheels;
            var requestedLinear = rules.WheelRadius * (movement.LeftRotatingVelocity.Radian + movement.RightRotatingVelocity.Radian) / 2;

			var linear = requestedLinear; // Math.Sign(requestedLinear) * Math.Min(Math.Abs(requestedLinear), rules.LinearVelocityLimit);
			var angular = requestedAngular;// Math.Sign(requestedAngular) * Math.Min(Math.Abs(requestedAngular), rules.AngularVelocityLimit.Radian);
			var angle = actor.World.Engine.GetAbsoluteLocation(actor.ObjectId).Yaw.Radian;
            
            //convert into cvarc world velocity
            
            var unitSpeed = new AIRLab.Mathematics.Frame3D(
               linear * Math.Cos(angle),
               linear * Math.Sin(angle),
               0,
               Angle.Zero,
               Angle.FromRad(angular),
               Angle.Zero);

			actor.World.Engine.SetSpeed(actor.ObjectId, unitSpeed);
            
            EncodersData encoderRecord = new EncodersData
            {
                Timestamp = actor.World.Clocks.CurrentTime,
                TotalLeftRotation = Angle.FromRad(movement.LeftRotatingVelocity.Radian * 0.005),
                TotalRightRotation = Angle.FromRad(movement.RightRotatingVelocity.Radian * 0.005)
            };
            actor.DWMData.EncodersHistory.Add(encoderRecord);
		}
	}
}
