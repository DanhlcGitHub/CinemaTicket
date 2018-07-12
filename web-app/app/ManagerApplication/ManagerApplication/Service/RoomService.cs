using ManagerApplication.CustomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ManagerApplication.Service
{
    interface IRoomService
    {
        int CreateRoom(Room entity);
    }
    public class RoomService : IRoomService
    {
        RoomRepository roomRepository = new RoomRepository();
        public List<Room> GetAll()
        {
            return roomRepository.GetAll();
        }
        public Room FindByID<E>(E id)
        {
            return roomRepository.FindByID(id);
        }
        public void Create(Room entity)
        {
            roomRepository.Create(entity);
        }
        public void Update(Room entity)
        {
            roomRepository.Update(entity);
        }
        public void Delete<E>(E id)
        {
            roomRepository.Delete(id);
        }
        public List<Room> FindBy(Expression<Func<Room, bool>> predicate)
        {
            return roomRepository.FindBy(predicate);
        }

        public int CreateRoom(Room entity)
        {
            return roomRepository.CreateRoom(entity);
        }
    }
}