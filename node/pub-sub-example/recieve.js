const amqplib = require('amqplib');


(async () => {

    const conn = await amqplib.connect('amqp://admin:admin@localhost')

    const ch2 = await conn.createChannel();

    const exchange = 'logs'

    assertExchangeRes = await ch2.assertExchange(exchange,'fanout',{
      durable:false
    })

    var queue = await ch2.assertQueue("",{exclusive:true})

    console.log(queue);

    await ch2.bindQueue(queue.queue, exchange)

    var consume_res = await ch2.consume(queue.queue,(msg) => {
      if(msg != null){
        console.log(msg.content.toString());
      }
    })

    console.log(consume_res);



    // await ch2.assertQueue(queue)

    // ch2.consume(queue, (msg) => {
    //     if(msg !== null){
    //         console.log(msg.content.toString());
    //         ch2.ack(msg)
    //     }else{
    //         console.log('consumer cancled by server')
    //     }
    // })

})()