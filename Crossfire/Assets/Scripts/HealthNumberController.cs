using UnityEngine;
using System.Collections;

public class HealthNumberController : IncrementingNumberController {

	public override void AddPressed () {
		ConvertStateToNumber ();
		base.AddPressed ();
		ConvertNumberToState ();
	}

	public override void SubtractPressed () {
		ConvertStateToNumber ();
		base.SubtractPressed ();
		ConvertNumberToState ();
	}

	void ConvertStateToNumber () {
		if (CurrentValue.text == "S") {
			CurrentValue.text = "0";
		} else if (CurrentValue.text == "C") {
			CurrentValue.text = "-1";
		}
	}

	void ConvertNumberToState () {
		if (CurrentValue.text == "0") {
			CurrentValue.text = "S";
		} else if (CurrentValue.text == "-1") {
			CurrentValue.text = "C";
		}
	}
}
