using Cinemachine;

namespace UI
{
    public static class Cameras
    {
        public static CinemachineVirtualCamera VirtualCamera
        {
            get
            {
                if (CinemachineCore.Instance.VirtualCameraCount == 0) return null;
                return (CinemachineVirtualCamera) CinemachineCore.Instance.GetVirtualCamera(0);
            }
        }
    }
}