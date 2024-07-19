using System.Threading.Tasks;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;

namespace NgocRongGold.Application.Threading
{
    public class Maintenance
    {
        private static Maintenance _instance;
        public int TimeCount { get; set; }
        public bool IsStart { get; set; }

        private Maintenance()
        {
            TimeCount = 3;
            IsStart = false;
        }

        public static Maintenance Gi()
        {
            return _instance ??= new Maintenance();
        }

        public void Start(int time)
        {
            TimeCount = time;
            IsStart = true;
            var task = new Task(Action);
            task.Start();
        }

        private async void Action()
        {
            
                ClientManager.Gi().SendMessageCharacter(Service.OpenUiSay(5, $"Máy chủ bảo trì sau {TimeCount} giây, vui lòng thoát trò chơi để đảm bảo không bị mất dữ liệu.\nBảo trì, trân trọng"));
                await Task.Delay(TimeCount * 1000);
            

            Server.Gi().StopServer();
        }
    }
}