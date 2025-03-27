public interface IMapper<in T, out T2>
{
    public T2 MapFrom(T obj);
}