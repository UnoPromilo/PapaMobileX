namespace PapaMobileX.Server.Mappers.Abstractions;

public interface IMapper<TIn, TOut>
{ 
    TOut Map(TIn input);
    IEnumerable<TOut> Map(IEnumerable<TIn> input);
}