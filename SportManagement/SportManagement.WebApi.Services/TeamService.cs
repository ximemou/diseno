using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SportManagement.Data;
using SportManagement.Data.Repository;
using SportManagement.Exceptions;

namespace SportManagement.WebApi.Services
{
    public class TeamService : ITeamService
    {

        private readonly IUnitOfWork unitOfWork;

        public TeamService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public Team CreateTeam(Team teamToCreate)
        {

            if (ExistsSport(teamToCreate.SportId))
            {
                if (teamToCreate != null)
                {
                    if (!ExistsTeamWithSameNameInSport(teamToCreate.SportId, teamToCreate.Name))
                    {
                        Sport sport = unitOfWork.SportRepository.GetByID(teamToCreate.SportId);
                        teamToCreate.Sport = sport;     
                        sport.Teams.Add(teamToCreate);
                        unitOfWork.TeamRepository.Insert(teamToCreate);
                        unitOfWork.Save();
                        return teamToCreate;
                    }
                    else
                    {
                        throw new NotUniqueException("Ya existe un equipo en el deporte con el nombre ingresado");
                    }
                }
                else
                {
                    throw new ArgumentException("El euqipo debe tener un nombre");
                }
            }
            else
            {
                throw new ArgumentException("No existe el deporte al cual se quiere agregar el equipo");
            }
        }

        private bool ExistsSport(int sportId)
        {
            Sport sport = unitOfWork.SportRepository.GetByID(sportId);
            if (sport != null)
                return true;
            return false;
        }

        private bool ExistsTeamWithSameNameInSport(int sportId,string teamName)
        {

            IEnumerable<Team> teams = unitOfWork.TeamRepository.Get(t => t.SportId == sportId && t.Name == teamName);
            if (teams.Count() > 0)
                return true;
            return false;
        }
        private bool ExistsTeam(int teamId)
        {
            Team team = unitOfWork.TeamRepository.GetByID(teamId);
            if (team == null)
                return false;
            return true;
        }


        

        private bool ExistsTeamWithSameName(string name)
        {
            IEnumerable<Team> teams = unitOfWork.TeamRepository.Get(t => t.Name == name);
            if (teams.Count()>0)
                return true;
            return false;
                
        }

        public bool DeleteTeam(int teamId)
        {
            Team team = GetTeamById(teamId);
            if (team != null)
            {
                DeleteGames(teamId);
                unitOfWork.TeamRepository.Delete(team);
                //FIAJARME SI SE BORRA SOLO DE LA TABLA USERTEAM
                unitOfWork.Save();
                return true;
            }
            return false;
        }

        private void DeleteGames( int teamId)
        {
            IEnumerable<Game> games = unitOfWork.GameRepository.Get(g => g.LocalTeam.TeamId == teamId || g.VisitingTeam.TeamId == teamId);
            foreach(Game game in games)
            {
                unitOfWork.GameRepository.Delete(game);
            }
        }

        public IEnumerable<Team> GetAllTeams()
        {
            IEnumerable<Team> teams = unitOfWork.TeamRepository.Get();
            return teams;
        }

        public Team GetTeamById(int teamId)
        {
            Team team = unitOfWork.TeamRepository.GetByID(teamId);
            return team;
        }

        private bool ExistsTeamWithSameNameInSportWhenUpdate(int sportId, string teamName,int teamId)
        {

            IEnumerable<Team> teams = unitOfWork.TeamRepository.Get(t => t.SportId == sportId && t.Name == teamName && t.TeamId!=teamId);
            if (teams.Count() > 0)
                return true;
            return false;
        }

        public Team UpdateTeam(int teamId, Team teamToUpdate)
        {
            Team team = unitOfWork.TeamRepository.GetByID(teamId);
            if (team != null)
            {
                if (!ExistsTeamWithSameNameInSportWhenUpdate(team.Sport.SportId, teamToUpdate.Name, teamId))
                {
                    team.Update(teamToUpdate);
                    unitOfWork.TeamRepository.Update(team);
                    return team;
                }
                else
                {
                    throw new NotUniqueException("Ya existe un equipo con el mismo nombre en el deporte");
                }
            }
            else
            {
                throw new NotExistsException("No existe el equipo que se quiere cambair");
            }
        }

       

        public IEnumerable<Team> GetAllTeamsInSport(int sportId)
        {
            if (ExistsSport(sportId))
            {
                IEnumerable<Team> teams = unitOfWork.TeamRepository.Get(t => t.SportId == sportId);
                return teams;
            }
            else
            {
                throw new ArgumentException("No existe el deporte buscado");
            }
        }

        public byte[] GetTeamPhoto(int teamId)
        {
            Team team = unitOfWork.TeamRepository.GetByID(teamId);
            if (team != null)
            {
                return team.Photo;
            }
            else
            {
                throw new NotExistsException("No existe el equipo");
            }
        }

        public void AddImageFile(int teamdId, IFormFile file)
        {
            Team team = unitOfWork.TeamRepository.GetByID(teamdId);
            if (team != null)
            {
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();

                }

                bool ok = GetImageFormat(fileBytes) != ImageFormat.unknown;
                if (ok)
                {
                    team.Photo = fileBytes;
                    unitOfWork.TeamRepository.Update(team);
                    unitOfWork.Save();
                }
                else
                {
                    throw new ArgumentException("Debe subir una imagen con formato jpeg,png,gif o tiff");
                }
            }
            else
            {
                throw new NotExistsException("No existe el equipo");

            }
        }

        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };              // PNG
            var tiff = new byte[] { 73, 73, 42 };                  // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };                 // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 };          // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 };         // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.jpeg;

            return ImageFormat.unknown;
        }

     

        public enum ImageFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }

    }
}
