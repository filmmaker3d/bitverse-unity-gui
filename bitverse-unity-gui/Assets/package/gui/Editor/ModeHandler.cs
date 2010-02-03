using System.Collections.Generic;


public interface ModeHandler
{
	void OnEnable();

	void OnDisable();

	void Execute();

	List<Shortcut> Shortcuts { get; }
}