using RailwayReservationMVC.Models.DAL;
using RailwayReservationMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
//using static RailwayReservationSystem.Models.DAL.RailwayRepository;

namespace RailwayReservationMVC.Models.DAL
{
    public class RailwayRepository<T> : IRailwayRepository<T> where T : class
    {

        private DataContext _context;
        private IDbSet<T> dbEntity;

        public RailwayRepository()
        {
            _context = new DataContext();
            dbEntity = _context.Set<T>();
        }
        public void DeleteModel(int ModelId)
        {
            T model = dbEntity.Find(ModelId);
            dbEntity.Remove(model);
        }

        public void DeletestringModel(string pnr)
        {
            T model = dbEntity.Find(pnr);
            dbEntity.Remove(model);
        }

        public IEnumerable<T> GetModel()
        {
            return dbEntity.ToList();
        }

        public T GetModelByID(int ModelId)
        {
            return dbEntity.Find(ModelId);
        }

        public void InsertModel(T model)
        {
            dbEntity.Add(model);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateModel(T model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }
    }
}
