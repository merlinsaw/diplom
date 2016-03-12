using UnityEngine;
using System.Collections;

public class PlayCellSound : ActionBase {
  public override void Act() {
    GetComponent<AudioSource>().Play();
  }
}
