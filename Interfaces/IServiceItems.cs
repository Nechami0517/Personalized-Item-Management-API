using project.Interfaces;

public interface IServiceItems<T> : IService<T>
{
  
    void deleteUsersItem(int id);
}