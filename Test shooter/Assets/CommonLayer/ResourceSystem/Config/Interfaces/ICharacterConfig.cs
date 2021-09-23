using CommonLayer.ResourceSystem.Config.Data;

namespace CommonLayer.ResourceSystem.Config.Interfaces
{
    public interface ICharacterConfig : IConfig
    {
        bool CursorVisible {get;}
        
        FloatRestraintParam Sensitivity {get;}
        
        bool VerticalInverted {get;}
        
        float VerticalRestraint {get;}
        
        float LookSmoothFactor {get;}
        
        FloatRestraintParam Speed {get;}
        
        float SitSpeedFactor {get;}
        
        float Gravity {get;}
        
        float MovementSmoothFactor {get;}
        
        float JumpHeight {get;}
        
        float GroundedVelocityY {get;}
        
        float SitPointOffsetY {get;}
        
        float StepOffSet {get;}
    }
}