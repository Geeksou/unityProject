using UnityEngine;
using System.Collections;

public enum PlayerAnimState{
	PARADO,
	CAINDO,
	PULANDO,
	ANDANDO
}

public class PlayerAnim : MonoBehaviour {

public PlayerAnimState atualState;
public float _velX;
public float _velY;
public bool _solo;


void Update () 
	{



		
		switch(atualState)
			{
			case PlayerAnimState.PARADO:
				{
				
				}
				break;
			case PlayerAnimState.CAINDO:
				{
				
				}
				break;
			case PlayerAnimState.PULANDO:
				{
				
				}
				break;
			case PlayerAnimState.ANDANDO:
				{
				
				}
				break;
			
			}
			
			
	}

}
