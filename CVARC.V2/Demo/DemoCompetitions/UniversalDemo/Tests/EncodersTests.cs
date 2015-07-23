using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo.UniversalDemo.Tests
{
    partial class DemoLogicPartHelper
    {
        DemoTestEntry EncodersTest(EncodersData encoders, params DemoCommand[] command)
        {
            return (client, world, asserter) =>
            {
                DemoSensorsData data = new DemoSensorsData();
                foreach (var c in command)
                    data = client.Act(c);
                asserter.IsEqual(encoders.TotalLeftRotation.Radian, data.Encoders.Sum(x => x.TotalLeftRotation.Radian), 0);
                asserter.IsEqual(encoders.TotalRightRotation.Radian, data.Encoders.Sum(x => x.TotalRightRotation.Radian),0);
            };
        }
        void LoadEncodersTests(LogicPart logic, DemoRules rules)
        {
            logic.Tests["Encoder_MoveForward"] = new RoundMovementTestBase(
                EncodersTest(
                new EncodersData { 
                    Timestamp = 0, 
                    TotalLeftRotation = Angle.FromGrad(114.3), 
                    TotalRightRotation = Angle.FromGrad(114.3) },
                rules.DWMMoveForward(10.0), rules.DWMStand(1.0)));
            logic.Tests["Encoder_ArcMoving"] = new RoundMovementTestBase(
                EncodersTest(
                new EncodersData
                {
                    Timestamp = 0,
                    TotalLeftRotation = Angle.FromGrad(234.0),
                    TotalRightRotation = Angle.FromGrad(52.0)
                },
                rules.DWMMoveArc(3.0, Angle.HalfPi, true)));
        }
    }

}
