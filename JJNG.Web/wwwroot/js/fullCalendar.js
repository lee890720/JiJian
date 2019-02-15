<script>  
    /**Demo for 3.0.1，It's different with version 2.*. */  
    $(document).ready(function() {  
          
        $('#calendar').fullCalendar({  
            /**  
             *  默认显示的视图  
             *  month       一页显示一月, 日历样式  
                basicWeek   一页显示一周, 无特殊样式  
                basicDay    一页显示一天, 无特殊样式  
                agendaWeek  一页显示一周, 显示详细的24小时表格  
                agendaDay   一页显示一天, 显示详细的24小时表格  
                listYear    年列表（V3.*支持）  
                listMonth   月列表（V3.*支持）  
                listWeek    周列表（V3.*支持）  
                listDay     日列表（V3.*支持）  
             */  
            defaultView:"agendaWeek",   //进入组件默认渲染的视图，默认为month  
            customButtons: {            //自定义header属性中按钮[customButtons与header并用]  
                myCustomButton: {  
                    text: 'custom!',  
                    click: function() {  
                        alert('clicked the custom button!');  
                    }  
                }  
            },  
            header: {  
                left: 'prev,next today',            //上一页、下一页、今天  
                center: 'title myCustomButton',     //居中：时间范围区间标题  
                right: 'month,agendaWeek,agendaDay,listWeek'    //右边：显示哪些视图  
            },  
            //          height : 500,           //组件高度，默认aspectRatio即纵横比；parent父容器大小；auto自动不带滚动条；支持数字和函数返回像素；支持$('#calendar').fullCalendar('option', 'height', 700);  
            //          contentHeight : 200,    //组件中的内容高度，默认aspectRatio即纵横比；auto自动不带滚动条；支持数字和函数返回像素；支持$('#calendar').fullCalendar('option', 'contentHeight', 700);  
            //          aspectRatio : 2,        //宽度:高度 比例，默认1.35，可自定义  
            //          handleWindowResize : false, //组件随浏览器高宽变化自适应，默认true支持自适应  
            //          windowResizeDelay : 200,//窗体自适应延迟多少毫秒  
            firstDay : 1,           //视图从每周几开始，默认0为周日，1为周1，2为周2，依此类推  
            isRTL : false,          //从右到左显示模式，默认false  
            //          weekends : false,       //是否在视图中显示周六、周日，默认true  
            //          hiddenDays: [ 1,5 ],    //隐藏星期1、星期5  
            fixedWeekCount : false, //月视图下，显示6周（不够的显示下个月的）true；默认true  
            weekNumbers : true,     //是否在视图左边显示这是第多少周，默认false  
            //          businessHours : {}      //设置哪些时间段重点标记颜色  
            eventLimit: true,       //数据条数太多时，限制各自里显示的数据条数（多余的以“+2more”格式显示），默认false不限制,支持输入数字设定固定的显示条数  
            //          eventLimitClick : "day",//接eventLimit属性，多余的内容点击事件，默认有一套弹窗格式，支持跳转到不同视图、也支持自定义function，详情见官方文档  
            viewRender : function(view,element){//在视图被渲染时触发（切换视图也触发） 参数view为视图对象，参数element为Jquery的视图Dom  
                console.log("↓↓↓viewRender↓↓↓");  
                console.log("name:"+view.name+"|title:"+view.title+"|start:"+view.start+"|end:"+view.end+"|intervalStart:"+view.intervalStart+"|intervalEnd:"+view.intervalEnd);  
            },  
            //          viewDestroy : function(view, element){}, //类似viewRender，在视图被销毁时触发  
            dayRender : function(date, cell){//日期格式Render钩子函数（我没理解清楚）  
                console.log("↓↓↓dayRender↓↓↓");  
                console.log(date);  
                console.log(cell);  
            },  
            windowResize : function(){//浏览器窗体变化时触发  
                console.log("---windowResize---");  
            },  
            //          allDaySlot : false,   //视图在周视图、日视图顶部显示“全天”信息，默认true显示全天  
            allDayText : "全天",            //自定义全天视图的名称  
            slotDuration : "01:00:00",      //一格时间槽代表多长时间，默认00:30:00（30分钟）  
            slotLabelFormat : "H(:mm)a",    //日期视图左边那一列显示的每一格日期时间格式  
            slotLabelInterval : "02:00:00", //日期视图左边那一列多长间隔显示一条日期文字(默认跟着slotDuration走的，可自定义)  
            snapDuration : "05:00:00",      //其实就是动态创建一个日程时，默认创建多长的时间块  
            //          scrollTime : "06:00:00",        //多远开始往下滚动，默认6小时，这个没看懂  
            //          minTime : "02:00:00",           //周/日视图左边时间线显示的最小日期，默认00:00:00  
            //          maxTime : "08:00:00",           //周/日视图左边时间线显示的最大日期，默认24:00:00  
            slotEventOverlap : false,       //相同时间段的多个日程视觉上是否允许重叠，默认true允许  
            //          listDayFormat : false,          //listview视图下，每天的分割区，[左边]的标题自定义，false表示无标题  
            //          listDayAltFormat : false,       //listview视图下，每天的分割区，[右边]的标题自定义，false表示无标题  
            noEventsMessage : "L没数据啊",   //listview视图下，无数据时显示提示  
            defaultDate  : '2016-10-13',    //默认显示那一天的日期视图  
            nowIndicator : true,            //周/日视图中显示今天当前时间点（以红线标记），默认false不显示  
            //          locale :  "zh-cn",              //国际化，前提引用lang-all.js，参见官方demo-->languages.html  
            //          timeFormat :  "h:mm",           //全局的日期显示格式(自定义成如12:00或12am等)  
            //          columnFormat : "",              //顶部日期显示格式'ddd' like 'Mon','ddd M/D' like 'Mon 9/7','dddd' like 'Monday'  
            //          titleFormat : "",               //顶部title区域格式化  
            buttonText : {today:'今天',month:'月',week:'周',day:'日',listWeek:'列表'},  //对应顶部操作按钮的名称自定义  
            monthNames : ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"], //月份自定义命名  
            monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"], //月份缩略命名（英语比较实用：全称January可设置缩略为Jan）  
            dayNames: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],       //同理monthNames  
            dayNamesShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],  //同理monthNamesShort  
            weekNumberTitle : "周",         //周的国际化,默认为"W"  
            //          displayEventTime : false,       //每一个日程块中是否显示时间，默认true显示  
            //          displayEventEnd : true,         //是否显示日程块中的“结束时间”，默认true，如果false则只显示开始时间  
            eventLimitText  : "更多",       //当一块区域内容太多以"+2 more"格式显示时，这个more的名称自定义（应该与eventLimit: true一并用）  
            dayPopoverFormat : "YYYY年M月d日", //点开"+2 more"弹出的小窗口标题，与eventLimitClick可以结合用  
            navLinks : true,                // “xx周”是否可以被点击，默认false，如果为true则周视图“周几”被点击之后进入日视图。本地测试：没什么效果  
            //          navLinkDayClick: function(date, jsEvent) { //依赖navLinks : true ， 点击顶部“日”时触发  
            //              console.log("↓↓↓navLinkDayClick↓↓↓");  
            //              console.log('day', date.format()); // date is a moment  
            //              console.log('coords', jsEvent.pageX, jsEvent.pageY);  
            //              return true;  
            //          },  
            //          navLinkWeekClick: function(weekStart, jsEvent) { //依赖navLinks : true ， 点击顶部“周”时触发  
            //              console.log("↓↓↓navLinkWeekClick↓↓↓");  
            //              console.log('week start', weekStart.format()); // weekStart is a moment  
            //              console.log('coords', jsEvent.pageX, jsEvent.pageY);  
            //          },  
            dayClick: function(date, jsEvent, view) {//空白的日期区，单击时触发  
                console.log("↓↓↓dayClick↓↓↓");  
                console.log('Clicked on: ' + date.format());  
                console.log('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);  
                console.log('Current view: ' + view.name);  
                // change the day's background color just for fun  
                $(this).css('background-color', 'red');  
            },  
            eventClick: function(calEvent, jsEvent, view) {//日程区块，单击时触发  
                console.log("↓↓↓eventClick↓↓↓");  
                console.log('Event: ' + calEvent.title);  
                console.log('Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY);  
                console.log('Current view: ' + view.name);  
                // change the day's background color just for fun  
                $(this).css('background-color', 'green');  
                return false;  //return false可以阻止点击后续事件发生（比如event中的url跳转事件）  
            },  
            eventMouseover: function(calEvent, jsEvent, view){//鼠标在日程区块上时触发  
                $(this).css('background-color', 'gray');  
            },  
            eventMouseout: function(calEvent, jsEvent, view){//鼠标从日程区块离开时触发  
                $(this).css('background-color', 'yellow');  
            },  
            selectable: true,           //允许用户可以长按鼠标选择多个区域(比如月视图，可以选中多天；日视图可以选中多个小时)，默认false不能选择多区域的  
            selectHelper: true,         //接selectable，周/日视图在选择时是否预先画出“日程区块”的样式出来  
            unselectAuto : true,        //是否点击页面上的其他地方会导致当前的选择被清除，默认true  
            unselectCancel : "",        //一种方法来指定元素，会忽略unselectauto选项，默认''  
            selectOverlap : true,       //确定用户是否被允许选择被事件占用的时间段，默认true可占用时间段  
            //          selectConstraint : ,        //限制用户选择指定时间段的日程数据：an event ID, "businessHours", object  
            selectAllow : function(selectInfo){ //精确的编程控制用户可以选择的地方，返回true则表示可选择，false表示不可选择  
                console.log("↓↓↓selectConstraint↓↓↓");  
                console.log("start:"+selectInfo.start+"|end:"+selectInfo.end+"|resourceId:"+selectInfo.resourceId);  
                return true;  
            },  
            select: function(start, end,jsEvent,view) { //点击空白区域/选择区域内容触发  
                console.log("↓↓↓select↓↓↓");  
                console.log("start:"+start+"|end:"+end+"|jsEvent:"+jsEvent+"|view:"+view.title);  
                var title = prompt('Event Title:');  
                var eventData;  
                if (title) {  
                    eventData = {  
                        title: title,  
                        start: start,  
                        end: end  
                    };  
                    $('#calendar').fullCalendar('renderEvent', eventData, true); // stick? = true  
                }  
                $('#calendar').fullCalendar('unselect');  
            },  
            unselect : function(view, jsEvent){//选择操作取消时触发  
                console.log("↓↓↓unselect↓↓↓");  
                console.log("view:"+view);  
            },  
            /**Event Object配置start */  
            //          allDayDefault : null,       //是否默认将日程全部显示在“全天”里面(boolean or null)，默认为undefined，即根据event时间值智能判断，这个属性太强悍，不敢用  
            //          startParam:"start",         //Event Object中定义[开始时间]的变量，默认是start，可自定义变量名以防冲突  
            //          endParam:"end",             //Event Object中定义[结束时间]的变量，默认是end，可自定义变量名以防冲突  
            //          timezoneParam : "timezone", //Event Object中定义[时区]的变量，默认是本地时区，可自定义变量名以防冲突，可更改时区如（ "America/Chicago" or "UTC"）  
            lazyFetching : true,        //是否启用懒加载技术--即只取当前条件下的视图数据，其它数据在切换时触发，默认true只取当前视图的，false是取全视图的  
            defaultTimedEventDuration : "02:00:00",     //在Event Object中如果没有end参数时使用，如start=7:00pm，则该日程对象时间范围就是7:00~9:00  
            defaultAllDayEventDuration : { days: 1 },  //默认1天是多长，（有的是采用工作时间模式，所以支持自定义）  
            //          forceEventDuration : false,     //诉老夫无能为力，愣是没搞懂什么意思，默认false  
            //          eventDataTransform : function(eventData){return [events...]}, //当从第三方取数不规则时（如下面的JSON或google），可通过此钩子函数进行数据整理，转换为fullcalendar识别的event object  
            loading : function(isLoading, view){ //视图数据加载中、加载完成触发  
                console.log("↓↓↓loading↓↓↓");  
                if(isLoading == true){  
                    console.log("view:"+view+",开始加载");  
                }else if(isLoading == false){  
                    console.log("view:"+view+",加载完成");  
                }else{  
                    console.log("view:"+view+",除非天塌下来否则不会进这个分支");  
                }  
            },  
            //          nextDayThreshold : "09:00:00",  //当一个事件的结束时间跨越到另一天，最短的时间，它必须为它的渲染，如果它在这一天。  
            eventOrder : "title",           //多个相同的日程数据排序方式，String / Array / Function, 默认值: "title"  
            eventRender : function(event, element, view) {          //当Event对象开始渲染时触发  
                console.log("eventRender():"+event.title);  
            },  
            //          eventAfterRender : function( event, element, view ) { } //当Event对象结束渲染时触发  
            eventAfterAllRender : function(view){console.log("eventAfterAllRender();");},   //当所有Event对象渲染完成时触发  
            //          eventDestroy : function( event, element, view ) { }     //一个Event DOM销毁时触发  
  
            /**Event Object配置end */  
  
            /**Event Rendering配置(一些样式等配置) start*/  
            //          eventColor: '#378006',      //不解释，所有的日程区块生效，如要对指定数据源生成参见EventSource，如要对指定Event生效，参见EventObject  
            //          eventBackgroundColor:"",    //同上，不解释  
            //          eventBorderColor:"",        //同上，不解释  
            //          eventTextColor:""           //同上，不解释  
  
            /**Event Rendering配置 end*/  
  
            editable: true,                 //支持Event日程拖动修改，默认false  
            eventStartEditable : true,      //Event日程开始时间可以改变，默认true，如果是false其实就是指日程块不能随意拖动，只能上下拉伸改变他的endTime  
            eventDurationEditable : false,  //Event日程的开始结束时间距离是否可以改变，默认true，如果是false则表示开始结束时间范围不能拉伸，只能拖拽  
            //          dragRevertDuration : 500,       //拖拽取消恢复花费时间，单位毫秒，这个就用默认的差不多了  
            dragOpacity:0.2,                //拖拽时不透明度，0.0~1.0之间，数字越小越透明  
            dragScroll : true,              //是否在拖拽时自动移动容器，默认true  
            eventOverlap : true,            //拖拽时是否重叠  
            eventConstraint : {     //限制拖拽拖放的位置（即限制有些地方拖不进去）：an event ID, "businessHours", object  
                start: '10:00',     // a start time (10am in this example)  
                end: '18:00',       // an end time (6pm in this example)  
                dow: [ 1, 2, 3, 4 ] // days of week. an array of zero-based day of week integers (0=Sunday)  (Monday-Thursday in this example)  
            },  
            longPressDelay : 1000,  //面向可touch设备（如移动设备），长按多少毫秒即可拖动,默认1000毫秒（1S）  
            eventDragStart : function(event, jsEvent, ui, view){    //日程开始拖拽时触发  
                console.log("eventDragStart():"+event.title);  
            },  
            eventDragStop : function(event, jsEvent, ui, view){     //日程拖拽停止时触发  
                console.log("eventDragStop():"+event.title);  
            },  
            eventDrop : function(event, delta, revertFunc, jsEvent, ui, view){  //日程拖拽停止并且已经拖拽到其它位置了  
                console.log("eventDrop():"+event.title);  
            },  
            eventResizeStart : function( event, jsEvent, ui, view ) {       //日程大小调整开始时触发  
                console.log("eventResizeStart():"+event.title);  
            },  
            eventResizeStop : function(event, jsEvent, ui, view){           //日程大小调整停止时触发  
                console.log("eventResizeStop():"+event.title);  
            },  
            eventResize : function(event, delta, revertFunc, jsEvent, ui, view){    //日程大小调整完成并已经执行更新时触发  
                console.log("eventResize():"+event.title);  
            },  
            /*          访问后台URL，动态返回JSON数据的形式，这种是直接集成了jQuery.ajax，其实并不灵活  
                        events:  {    
                            url: '/myfeed.php',  
                            type: 'POST',  
                            data: {  
                                custom_param1: 'something',  
                                custom_param2: 'somethingelse'  
                            },  
                            error: function() {  
                                alert('there was an error while fetching events!');  
                            },  
                            color: 'yellow',   // a non-ajax option  
                            textColor: 'black' // a non-ajax option  
                        },  
            */  
            /*          第二种：基于function的数据获取，通常是在切换上一页、下一页、视图切换时触发，非常适合动态数据获取  
                        events: function(start, end, timezone, callback) {  
                        $.ajax({  
                            url: 'myxmlfeed.php',  
                            dataType: 'xml',  
                            data: {  
                                // our hypothetical feed requires UNIX timestamps  
                                start: start.unix(),  
                                end: end.unix()  
                            },  
                            success: function(doc) {  
                                var events = [];  
                                $(doc).find('event').each(function() {  
                                    events.push({  
                                        title: $(this).attr('title'),  
                                        start: $(this).attr('start') // will be parsed  
                                    });  
                                });  
                                callback(events);  
                            }  
                        });  
                        },  
            */  
            events: [//第三种：直接是数组的形式传入  
                {  
                    title: 'All Day Event',  
                    start: '2016-10-13'  
                },  
                {  
                    title: 'Long Event',  
                    start: '2016-10-07',  
                    end: '2016-10-10'  
                },  
                {  
                    id: 991,  
                    title: 'Repeating Event',  
                    start: '2016-10-09T16:00:00'  
                },  
                {  
                    id: 999,  
                    title: 'Repeating Event',  
                    start: '2016-10-16T16:00:00'  
                },  
                {  
                    title: 'Conference',  
                    start: '2016-10-11',  
                    end: '2016-10-13'  
                },  
                {  
                    title: 'Meeting',  
                    start: '2016-10-12T10:30:00',  
                    end: '2016-10-12T12:30:00'  
                },  
                {  
                    title: 'Lunch',  
                    start: '2016-10-12T12:00:00'  
                },  
                {  
                    title: 'Meeting',  
                    start: '2016-10-12T14:30:00'  
                },  
                {  
                    title: 'Happy Hour',  
                    start: '2016-10-12T17:30:00'  
                },  
                {  
                    title: 'Dinner',  
                    start: '2016-10-12T20:00:00'  
                },  
                {  
                    title: 'Birthday Party',  
                    start: '2016-10-13T07:00:00'  
                },  
                {  
                    title: 'Click for Google',  
                    url: 'http://google.com/',  
                    start: '2016-10-28'  
                }  
            ]  
  
            /*          多数据源支持:场景可能是从多种模块取数据，比如从会议取会议日程、从计划取计划日程，而且要求会议、计划日程颜色显示不一样，那么就可以用这种  
                        eventSources: [  
                            {  
                                events: function(start, end, timezone, callback){...},  
                                color: 'black',     // an option!  
                                textColor: 'yellow' // an option!  
                            },  
                            {  
                                googleCalendarId: 'abcd1234@group.calendar.google.com',  
                                color: 'yellow',   // an option!  
                                textColor: 'black' // an option!  
                            }  
                        ]  
            */  
          
  
        });  
          
        //      日期格式需要遵守fullCalendar引用的moment.js规则：https://fullcalendar.io/docs/utilities/Moment/  
  
        //      Event Object 就是一个日程区块，数据元，通常以数组的形式传入option->events中 见 https://fullcalendar.io/docs/event_data/Event_Object/  
        //      Event Source Object 不好理解，大概就是一组日程数据源对象吧，可以是一个events、也可以是JSON、还可以是Google Calendar的数据 见 https://fullcalendar.io/docs/event_data/Event_Source_Object/  
  
        /* updateEvent更新一个日程对象  
        $('#calendar').fullCalendar({  
            eventClick: function(event, element) {  
                event.title = "CLICKED!";  
                //更新日程对象信息  
                $('#calendar').fullCalendar('updateEvent', event);  
            }  
        });  
        */  
  
        //      .fullCalendar( 'clientEvents' [, idOrFilter ] ) ->   Array  从内存中筛选指定的event,[, idOrFilter ]==>省略号删除全部、ID数组删除有ID的日程、也可传入Event对象，建议使用时详细查看官方文档  
        //      .fullCalendar( 'removeEvents' [, idOrFilter ] )     删除日程，[, idOrFilter ]参见clientEvents，建议使用时详细查看官方文档  
        //      .fullCalendar( 'refetchEvents' )                    刷新所有数据源的日历视图，建议使用时详细查看官方文档  
        //      .fullCalendar( 'refetchEventSources', sources )     刷新指定数据源的日历视图（与eventSource有关配合用），建议使用时详细查看官方文档  
        //      .fullCalendar( 'addEventSource', source )           动态添加一个数据源（与eventSource有关配合用），建议使用时详细查看官方文档  
        //      .fullCalendar( 'removeEventSource', source )        动态删除一组数据源（与eventSource有关配合用），建议使用时详细查看官方文档  
        //      .fullCalendar( 'removeEventSources', optionalSourcesArray ) 动态删除多个数据源，如果optionalSourcesArray未指定则删除全部，建议使用时详细查看官方文档  
        //      .fullCalendar( 'getEventSources' )                  返回所有的数据源  
        //      .fullCalendar( 'getEventSourceById', id )           根据ID获取数据源  
  
        //      $('#calendar').fullCalendar('render');  渲染日历视图  
        //      $('#calendar').fullCalendar('destory'); 销毁日历视图  
  
        //      var view = $('#calendar').fullCalendar('getView'); //获取当前视图对象  
        //      console.log("name:"+view.name+"|title:"+view.title+"|start:"+view.start+"|end:"+view.end+"|intervalStart:"+view.intervalStart+"|intervalEnd:"+view.intervalEnd);  
        //      $('#calendar').fullCalendar('changeView', "month" ); //切换为其它视图  
        //      $('#calendar').fullCalendar('prev'); //切换到当前视图的上一页（类似于顶部的“<”箭头）  
        //      $('#calendar').fullCalendar('next'); //切换到当前视图的下一页（类似于顶部的“>”箭头）  
        //      $('#calendar').fullCalendar('prevYear') //切换到上一年  
        //      $('#calendar').fullCalendar('nextYear') //切换到下一年  
        //      $('#calendar').fullCalendar('today');   //日期视图跳转到“今天”  
        //      $('#calendar').fullCalendar( 'gotoDate', date );  //日期视图跳转到指定时间  
        //      $('#calendar').fullCalendar( 'incrementDate', duration ); //日期视图向前或向后移动固定的时间，duration可以为={ days:1, hours:23, minutes:59 }  
  
        //      var moment = $('#calendar').fullCalendar('getDate');      //获取当前视图的日期对象，（如果月视图则返回月初到月末，周视图返回周初到周末）  
        //      console.log("The current date of the calendar is " + moment.format());  
        //      $('#calendar').fullCalendar( 'select', start, [ end ], [ resource ] ); //js动态选择某个时间段的日程  
        //      .fullCalendar( 'unselect' );  //js动态取消选择的日程  
  
        //      var locale = $('#calendar').fullCalendar('option', 'locale');        //option支持get，注：不仅限于locale，还包括其它option操作  
        //      $('#calendar').fullCalendar('option', {locale: 'fr',isRTL: true});   //option支持set，注：不仅限于locale，还包括其它option操作  
  
        //      var calendar = $('#calendar').fullCalendar('getCalendar'); //支持动态绑定/解绑fullcalendar中的事件  
        //      calendar.on('dayClick', function(date, jsEvent, view) {console.log('clicked on ' + date.format());});  
  
        //      .fullCalendar( 'renderEvent', event [, stick ] );   //渲染一个新的Event，建Demo select方法  
        //      .fullCalendar( 'rerenderEvents' )                   //渲染那所有的Event  
  
  
  
    });  
  
</script>  