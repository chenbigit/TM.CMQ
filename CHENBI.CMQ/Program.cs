using System;
using System.Collections.Generic;

namespace CHENBI.CMQ
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string secretId = "";
            string secretKey = "";
            string endpoint = "http://cmq-queue-gz.api.qcloud.com";
            string path = "/v2/index.php";

            Account account = new Account(secretId, secretKey, endpoint,path);
            QueueMeta meta = new QueueMeta();
            //创建队列
            account.CreateQueue("q11", meta);
            CMQClient cMQClient = new CMQClient(endpoint, path, secretId, secretKey, "POST");
            Queue queue = new Queue("q11", cMQClient);
            var msgId= queue.SendMessage("ceshi message");

            //消费一条消息
            var resReceive= queue.ReceiveMessage(30);
            //删除消息
            queue.DeleteMessage(resReceive.ReceiptHandle);

            //获取队列列数
            List<QueueInfo> list = new List<QueueInfo>();
            var count = account.ListQueue("", 1, 1, list);



            Topic topic = new Topic("topc1", cMQClient);
            topic.PublishMessage("messsssss");



            Subscription subscription = new Subscription("topc1", "subscription1", cMQClient);
            subscription.GetSubscriptionAttributes();

            Console.WriteLine("Hello World!");
        }
    }
}
