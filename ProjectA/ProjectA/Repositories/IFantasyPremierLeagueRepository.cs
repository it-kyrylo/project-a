using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectA.Repositories
{
    public interface IFantasyPremierLeagueRepository
    {
        [Get("/api/bootstrap-static/")]
        Task<string> LoadBootstrapStaticData();
    }
}
