using Godot;
using System;

public class Bullet : RigidBody
{
	float timer = 0;
    [Export]
    public int damage = -1;
    [Export]
    public float speed = 15;
    [Export]
    public float lifetime = 15;
    public bool friendly = false;
    [Signal]
    public delegate void DealDamage(byte damagePoints);
    [Export]
    public PackedScene sparks = (PackedScene)ResourceLoader.Load("res://Prefabs/sparks.tscn");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        timer+= delta;
		if (timer > lifetime) QueueFree();
    }
	private void _OnCollisionEnter(Node body)
	{
        if (friendly && body.IsInGroup("Players")) //this if statement doesnt really have to exist but if I ever want to make a multiplayer mode and prevent friendly fire, this better exist.
        {
            return;
        }
        /*if (!friendly && body.IsInGroup("Enemies")) //this if statement doesnt really have to exist but if I ever want to make a multiplayer mode and prevent friendly fire, this better exist.
        {
            return;
        }*/ // un comment this statement to disable monster infighting
        if (body.HasMethod("UpdateHealth"))
        {
            Connect(nameof(DealDamage), body, "UpdateHealth");
            EmitSignal(nameof(DealDamage), damage);
        }

            Spatial newSparks = (Spatial)sparks.Instance();
            newSparks.Translation = Translation;
            GetTree().Root.AddChild(newSparks);
            
            //newSparks.Rotation = Rotation;
    	QueueFree();
	}
    
}

