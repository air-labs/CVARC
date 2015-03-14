using System;
using System.Web.Mvc;
using ServerReplayPlayer.Contracts;

namespace ServerReplayPlayer.Controllers
{
    public class HomeController : BaseController
    {
        public string[] Levels { get { return new[] { "Level1", "Level2" }; } }

        public ActionResult Index()
        {
            return View(ReadReplays());
        }

        private ReplaysViewModel[] ReadReplays()
        {
            return new[]
                {
                    new ReplaysViewModel
                        {
                            Level = "Level1",
                            Replays = new[]
                                {
                                    new Summary(Guid.NewGuid(), "10", "Вася"), 
                                    new Summary(Guid.NewGuid(), "11", "Вася2"), 
                                    new Summary(Guid.NewGuid(), "20", "Вася3"), 
                                    new Summary(Guid.NewGuid(), "30", "Вася4"), 
                                    new Summary(Guid.NewGuid(), "50", "Вася5"), 
                                }
                        }, 
                    new ReplaysViewModel
                        {
                            Level = "Level2",
                            Replays = new[]
                                {
                                    new Summary(Guid.NewGuid(), "10", "Вася1", "qwe6"), 
                                    new Summary(Guid.NewGuid(), "20", "Вася2", "qwe5"), 
                                    new Summary(Guid.NewGuid(), "30", "Вася3", "qwe4"), 
                                    new Summary(Guid.NewGuid(), "40", "Вася4", "qwe3"), 
                                    new Summary(Guid.NewGuid(), "50", "Вася5", "qwe2"), 
                                    new Summary(Guid.NewGuid(), "60", "Вася6", "qwe1"), 
                                }
                        }, 
                };
        }
    }
}
