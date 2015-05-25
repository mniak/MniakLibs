
namespace Mniak.Network.Filters
{
	public interface IFilter
	{
		byte[] FilterSend(byte[] data);
		byte[] FilterReceive(byte[] data);
	}
}
