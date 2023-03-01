using System;
using System.Linq;
using UniRx;
using UnityEngine;

public class PlayerCharacter : CharacterBase {

    private InputController inputController;    
    private CompositeDisposable disposables = new CompositeDisposable();
        
    protected Vector2 AxisInput;

    public MouseMovement MouseMove { get; private set; }
    
    protected virtual void Start()
    {
        //Get input from input controller
        inputController = InputController.Instance;        
        inputController.Horizontal.Subscribe(horizontal => AxisInput.x = horizontal).AddTo(disposables);
        inputController.Vertical.Subscribe(vertical => AxisInput.y = vertical).AddTo(disposables);
        inputController.MouseMove.Subscribe(movement => MouseMove = movement).AddTo(disposables);
        inputController.Run.Subscribe(run => Running = run).AddTo(disposables);
        inputController.Fire1.Subscribe(fire => HandleShooting(fire)).AddTo(disposables);
        inputController.Jump.Subscribe(jump => HandleJumpInput(jump)).AddTo(disposables);
    }



    void OnDestroy() {
        disposables.Dispose();        
    }
        
    protected virtual void HandleShooting(bool isDown) { }
    protected virtual void HandleJumpInput(bool isDown) { }
}
