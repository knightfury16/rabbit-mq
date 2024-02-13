const amqplib = require('amqplib');


(async () => {
    const conn = await amqplib.connect('amqp://admin:admin@localhost');
    
    const ch1 = await conn.createChannel();
    
    
    const queue = 'tasks';
    const queue2 = 'myqueue';
    const msg = 'hello world'
    const msg2 = 'this msg is for myqueue'
    

    await ch1.assertQueue(queue);
    await ch1.assertQueue(queue2);
    
    ch1.sendToQueue(queue, Buffer.from(msg));
    ch1.sendToQueue(queue2,Buffer.from(msg2));

    setTimeout(function() {
    conn.close();
    process.exit(0)
    }, 500);

})()