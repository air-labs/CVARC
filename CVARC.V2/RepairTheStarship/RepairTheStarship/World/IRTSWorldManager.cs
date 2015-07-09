using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace RepairTheStarship
{
    public interface IRTSWorldManager : IWorldManager
    {
        void RemoveDetail(string detailId);
        void ShutTheWall(string wallId);
        void CreateEmptyTable();
        void CreateWall(string wallId, Point2D centerLocation, WallData settings);
        void CreateDetail(string detailId, Point2D detailLocation, DetailColor color);
    }
}
