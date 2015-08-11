using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using AIRLab.Mathematics;
using CVARC.V2;

namespace RoboMovies
{
    public interface IRMWorldManager : IWorldManager
    {
        void RemoveObject(string objectId);
        void CreateEmptyTable();
        void CloseClapperboard(string clapperboardId);
        void CreateStartingArea(Point2D centerLocation, SideColor color);
        void CreateStairs(string stairsId, Point2D centerLocation, SideColor color);
        void CreateLight(string lightId, Point2D location);
        void CreateStand(string standId, Point2D location, SideColor color);
        void CreateClapperboard(string clapperboardId, Point2D location, SideColor color);
        void CreatePopCorn(string popcornId, Point2D location);
        void CreatePopCornDispenser(string dispenserId, Point2D location);
        void ClimbUpStairs(string actorId, string stairsId);
    }
}
