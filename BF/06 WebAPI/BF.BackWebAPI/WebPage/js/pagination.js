define(['zepto'], function () {


    var pindex = 1;
    var psize = 10;
    var ptotal = 0;
    var pmaxindex = 0;

    function paging(total, size, callback) {

        if (size > 0) {
            psize = size;
        }
        //callback(pindex, psize);

        ptotal = Math.ceil(total / psize);
        var page_html = "<ul><li data='Prev'><a href='#'>Prev</a></li>";
        if (ptotal > 10) {
            if (pindex > 4 && pindex < ptotal - 4) {

            } else {

            }
        } else {
            for (var i = 0; i < ptotal; i++) {
                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
            }
        }
        page_html += " <li data='Next'><a href='#'>Next</a></li></ul>";
        $("#pagination").html(page_html);
        $("#pagination li").off('click');
        $("#pagination li").on('click', function () {
            var c_value = $(this).attr("data");
            if (c_value == "Prev") {
                pindex = (pindex - 1);
            } else if (c_value == "Next") {
                pindex = (pindex + 1);
            } else {
                pindex = c_value;
            }
            callback(pindex, psize);
        });
        //生成分页控件
        //if (pindex < pmaxindex && pmaxindex != 0) {
        //    //loading();
        //    pindex += 1;
        //    callback(pindex, psize);
        //}

    }


    function setindex(i, total) {
        if (i > 0)
            pindex = i;
        if (total) {
            pmaxindex = Math.ceil(total / psize);
            ptotal = total;
        }
    }

    return {
        paging: paging,
        setindex: setindex
    }
})