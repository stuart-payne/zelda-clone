using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBinaryState
{
	void SetBool(bool state);
	void Switch();
	bool GetBool();
}
