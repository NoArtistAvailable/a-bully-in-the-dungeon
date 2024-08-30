namespace Modules.CharacterController
{
	public interface IBlockable
	{
		public void Block(object blocker);
		public void Unblock(object blocker);
		//should have a List<object> of blockers
	}
}