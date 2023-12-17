using UnityEngine;

public class CollectableChangeMaterial : Collectable
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void OnCollect(PlayerController player)
    {
        base.OnCollect(player);

        Renderer r = GetComponent<MeshRenderer>();
        Renderer rPlayer = player.GetComponent<MeshRenderer>();
        rPlayer.material = r.material;
    }
}
