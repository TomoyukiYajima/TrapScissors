using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : MonoBehaviour
{

    public enum TrapCommand       //ボタンの待機時間はとりあえず1秒
    {
        //Normal,     //自分が噛みつく
        Command_Wake,       //仲間のとらばさみを起こす
        Command_Trap,       //仲間のとらばさみの状態変化
        Command_Move,       //仲間のとらばさみを動かす
        Command_Bite        //仲間のとらばさみを噛みつかせる
    }

    enum CommandLengh
    {
        Short,
        Long,
        None
    }

    public GameObject Player_2;

    private CommandLengh command;
    private int counter;
    private float time;
    public float shortTime = 10.0f;

    private Dictionary<string, TrapCommand> commands = new Dictionary<string, TrapCommand>();
    private string command_name = "";

    // Use this for initialization
    void Start()
    {
        command = CommandLengh.None;
        time = 0;
        shortTime = 1.0f / 60.0f * shortTime;
        commands.Add("SLSS", TrapCommand.Command_Wake);
        commands.Add("LSLS", TrapCommand.Command_Trap);
        commands.Add("SSL", TrapCommand.Command_Move);
        commands.Add("LLLS", TrapCommand.Command_Bite);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            command = CommandLengh.None;
            time = 0;
        }

        if (command == CommandLengh.None)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                time += Time.deltaTime;
                if (time < shortTime) return;

                command = CommandLengh.Long;
                print("Long");
                command_name += "L";
                return;
            }
            else if (Input.GetKeyUp(KeyCode.Space) && command == CommandLengh.None)
            {
                if (time >= shortTime) return;
                command = CommandLengh.Short;
                print("Short");
                command_name += "S";
            }
        }

        if(command_name.Length >= 3)
        {
            foreach(var i in commands.Keys)
            {
                if(command_name == i)
                {
                    if(command_name == "SLSS")
                    {
                        //print("Wake");
                        
                    }
                    print(commands[i].ToString());
                    command_name = "";
                    break;
                }
            }
            if(command_name.Length >= 4)
            {
                command_name = command_name.Remove(0, 1);
                print("delete");
            }
        }

    }
}
