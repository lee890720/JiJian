function showTop (current,width,height,content){
    $(".toolTip").toggle().css({"width":width,"height":height,     // 这里一定要先toggle再给定义样式 不然提示框第一次出现的时候没有宽高
        "left": $(current).offset().left - ($(".toolTip").width()/2 - $(current).width()/2),
        "top": $(current).offset().top - height - 2
    });
    tipContent (content)
};
function showBottom (current,width,height,content){
    $(".toolTip").toggle().css({"width":width,"height":height,
        "left": $(current).offset().left - ($(".toolTip").width()/2 - $(current).width()/2),
        "top": $(current).offset().top + $(current).height() + 2
    });
    tipContent (content)
};
function showLeft (current,width,height,content){
    $(".toolTip").toggle().css({"width":width,"height":height,
        "left": $(current).offset().left - width - 6,
        "top": $(current).offset().top - height/2 + $(current).height() / 2
    });
    tipContent (content)
};
function showRight (current,width,height,content){
    $(".toolTip").toggle().css({"width":width,"height":height,
        "left": $(current).offset().left + $(current).width() + 5,
        "top": $(current).offset().top - height/2 + $(current).height() / 2
    });
    tipContent (content)
};
function rightTop (current,width,height,content){
    $(".toolTip").toggle().css({"width":width,"height":height,
        "left": $(current).offset().left + $(current).width(),
        "top": $(current).offset().top - height
    });
    tipContent (content)
};
function leftTop (current,width,height,content){
    $(".toolTip").toggle().css({"width":width,"height":height,
        "left": $(current).offset().left - $(".toolTip").width(),
        "top": $(current).offset().top - height
    });
    tipContent (content)
};
function leftBottom (current,width,height,content){
    $(".toolTip").toggle().css({"width":width,"height":height,
        "left": $(current).offset().left - $(".toolTip").width(),
        "top": $(current).offset().top + $(current).height()
    });
    tipContent (content)
};
function rightBottom (current,width,height,content){
    $(".toolTip").toggle().css({"width":width,"height":height,
        "left": $(current).offset().left + $(current).width(),
        "top": $(current).offset().top + $(current).height()
    });
    tipContent (content)
}
function tipContent (content){
    //先清空再赋值
    $(".tipContent").html("");
    $(".tipContent").html(content);
};

function Tooltip(current,content,width,height){
    if(width > 200){
        width = 200;
        alert("最大宽度为200，已自动修改为200");
    }
    if(height < 40){
        height = 40;
        alert("最小高度为40，已自动修改为40");
    }
    // 首先给提示框的宽高赋值 方便以后调用
    $(".toolTip").css({"width":width +'px',"height":height +'px'});
    // 计算当前元素相对于窗口 上下左右的距离
    var leftDis = $(current).offset().left - $(document).scrollLeft();
    var rightDis = $(window).width() - leftDis - $(current).width();
    var topDis = $(current).offset().top - $(document).scrollTop();
    var bottomDis = $(window).height() - topDis - $(current).height();

    var tipWidth = parseInt($(".toolTip").css("width"));
    var tipHeight = parseInt($(".toolTip").css("height"));

    if(topDis >= height && leftDis >= tipWidth/2 && rightDis >= tipWidth/2 ){                   //在上方显示
        console.log("top");
        showTop(current,width,height,content);
    }else if(bottomDis >= height && leftDis >= tipWidth/2 && rightDis >= tipWidth/2){           // 在下方显示
        console.log("bottom");
        showBottom(current,width,height,content);
    }else if(leftDis >= width && topDis >= tipHeight/2 && bottomDis >= tipHeight/2 ){           // 在左边显示
        console.log("left");
        showLeft(current,width,height,content);
    }else if(rightDis >= width && topDis >= tipHeight/2 && bottomDis >= tipHeight/2 ){          // 在右边显示
        console.log("right");
        showRight(current,width,height,content);
    }else if(topDis >= height && leftDis < tipWidth/2 && rightDis >= tipWidth){                 // 在右上方显示
        console.log("rightTop");
        rightTop(current,width,height,content);
    }else if(topDis >= height && leftDis >= tipWidth && rightDis < tipWidth/2){                 // 在左上方显示
        console.log("leftTop");
        leftTop(current,width,height,content);
    }else if(bottomDis >= height && leftDis < tipWidth/2 && rightDis >= tipWidth){              // 在右下方显示
        console.log("rightBottom");
        rightBottom(current,width,height,content);
    }else if(bottomDis >= height && leftDis >= tipWidth && rightDis < tipWidth/2){              // 在左下方显示
        console.log("leftBottom");
        leftBottom(current,width,height,content);
    }else {                                                                                     // 默认在上方显示
        console.log("top2");
        showTop(current,width,height,content);
    }
}
$(function (){
    $(".close").click(function (){
        $(".toolTip").hide();
    });
});