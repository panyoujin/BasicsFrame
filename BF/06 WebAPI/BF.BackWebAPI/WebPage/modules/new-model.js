define(['domready!', 'zepto', 'common', 'angular', 'server_data/models_data'], function (doc, $, c, angular, m) {

    loading(0);
    var isedit = $('*[edit]').length > 0;
    var width = Screen.ViewWidth();
    var height = Screen.ViewHeight();
    var info = {
        model_Name: '',//模式名称，养生品名称
        final_Temperature: 100,//最终温度
        cook_Time: 5,//煮料时长，单位分钟
        cook_Temperature: 100,//煮料温度，单位摄氏度，最小单位1摄氏度
        removal_Chlorine_Time: 0,//除氯时长，单位分钟
        is_Heat_Preservation: false,//是否保温，默认不保温
        heat_Preservation_Time: 0,//保温时长，单位分钟
        heat_Preservation_Temperature: 0,//保温温度，单位摄氏度，最小单位1摄氏度
        isFerv: false,//是否煮沸,默认不煮沸
        isBubble: false,//是否泡料，默认不泡
        bubble_Temperature: 0,//泡料温度，单位摄氏度，最小单位1摄氏度
        bubble_Time: 0,//泡料时长，单位分钟
        sort: 0,//排序，越大越前
        icoUrl: "",//图标地址
        imageUrl: "",//图片地址
        introduce: "",//简单介绍，用户自定义的需要动态生成
        describe: "",//材料描述
        remarks: ""//注意事项
    }
    function bindEvent() {
        $('body').show();
        loading(0);

        $('#save_model').off('click')

        $('#save_model').on('click', function () {
            if (!info.model_Name) {
                alert('名称不能为空');
                return;
            }
            if (!info.final_Temperature) {
                alert('最终温度不能为空');
                return;
            }
            if (!info.cook_Time) {
                alert('煮料时间不能为空');
                return;
            }
            m.AddHealthModel(info, function (data) {
                alert(data.msg);
            })
        });
    }
    function init_data() {
        if (isedit) {

        } else {
            angular.set('info', info, bindEvent)
        }
    }
    $(function () {
        init_data();
    })

})