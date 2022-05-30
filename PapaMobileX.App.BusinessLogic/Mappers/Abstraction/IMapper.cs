namespace PapaMobileX.App.BusinessLogic.Mappers.Abstraction;

public interface IMapper<TIn, TOut>
{ 
    TOut Map(TIn input);
    IEnumerable<TOut> Map(IEnumerable<TIn> input);
}