using UnityEngine;
using UniRx;

public class MouseMovement
{
    public Vector2 Position;
    public Vector2 Delta;

    public MouseMovement() { }

    public MouseMovement(Vector2 _pos, Vector2 _delta)
    {
        Position = _pos;
        Delta = _delta;
    }
}

public class InputController : SingletonMono<InputController>
{
    private int captureIndex = 0;

    public ReactiveProperty<float> Horizontal = new ReactiveProperty<float>();
    public ReactiveProperty<float> Vertical = new ReactiveProperty<float>();

    public ReactiveProperty<MouseMovement> MouseMove = new ReactiveProperty<MouseMovement>();
    public ReactiveProperty<float> MouseHorizontal = new ReactiveProperty<float>();
    public ReactiveProperty<float> MouseVertical = new ReactiveProperty<float>();

    public ReactiveProperty<bool> Fire1 = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> Jump = new ReactiveProperty<bool>();

    public ReactiveProperty<bool> Run = new ReactiveProperty<bool>();

    public ReactiveProperty<bool> Anykey = new ReactiveProperty<bool>();

    public void Init()
    {
        UpdateProperties();    
    }

    private void Update()
    {
        UpdateProperties();
    }

    private void UpdateProperties()
    {
        //Input axes

        Horizontal.Value = Input.GetAxis("Horizontal");
        Vertical.Value = Input.GetAxis("Vertical");

        MouseHorizontal.Value = Input.GetAxis("Mouse X");
        MouseVertical.Value = Input.GetAxis("Mouse Y");

        MouseMove.Value = new MouseMovement(Input.mousePosition, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));

        //Buttons

        if (Input.GetButtonDown("Fire1"))
            Fire1.Value = true;

        if (Input.GetButtonUp("Fire1"))
            Fire1.Value = false;

        /*
        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Capture screenshot");
            ScreenCapture.CaptureScreenshot("Homage_capture" + captureIndex.ToString("0000") + ".png", 4);
            captureIndex++;
        }
        */
                
        if (Input.GetButtonDown("Jump"))
            Jump.Value = true;

        if (Input.GetButtonUp("Jump"))
            Jump.Value = false;

        //Keys

        Run.Value = Input.GetKey(KeyCode.LeftShift);
        Anykey.Value = Input.anyKey;
    }
}
