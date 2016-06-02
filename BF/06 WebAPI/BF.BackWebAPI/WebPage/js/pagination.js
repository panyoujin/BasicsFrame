define(['zepto'], function () {


    var pindex = 1;
    var psize = 10;
    var ptotal = 0;
    var pmaxindex = 0;

    function paging(index, size, callback) {

        if (size > 0) {
            psize = size;
        }
        if (index > 0) {
            pindex = index;
        }
        //ptotal = Math.ceil(total / psize);
        callback(pindex, psize);
        return;
        //ptotal = Math.ceil(total / psize);
        //var page_html = "<ul><li data='Prev'><a href='#'>Prev</a></li>";
        //if (ptotal > 10) {
        //    if (pindex <= 6) {
        //        for (var i = 0; i < ptotal; i++) {
        //            if ((i < 6 && i != (pindex - 1)) || i >= ptotal - 2) {
        //                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
        //            } else if (i == (pindex - 1)) {
        //                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
        //            } else {
        //                i = ptotal - 3;
        //                page_html += "<li data=''><a href='#'>...</a></li>";
        //            }
        //        }
        //    } else if (pindex > 6 && pindex <= 8) {
        //        for (var i = 0; i < ptotal; i++) {
        //            if ((i < 8 && i != (pindex - 1)) || i >= ptotal - 2) {
        //                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
        //            } else if (i == (pindex - 1)) {
        //                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
        //            } else {
        //                i = ptotal - 3;
        //                page_html += "<li data=''><a href='#'>...</a></li>";
        //            }
        //        }
        //    } else if (pindex >= ptotal - 5) {
        //        for (var i = 0; i < ptotal; i++) {
        //            if (i < 2 || (i > (pindex - 3) && i != (pindex - 1))) {
        //                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
        //            } else if (i == (pindex - 1)) {
        //                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
        //            } else {
        //                i = pindex - 4;
        //                page_html += "<li data=''><a href='#'>...</a></li>";
        //            }
        //        }
        //    } else {
        //        for (var i = 0; i < ptotal; i++) {
        //            if (i < 2 || i > ptotal - 2) {
        //                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
        //            } else if (i > pindex - 3 && i < pindex + 2 && i != (pindex - 1)) {
        //                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
        //            } else if (i == (pindex - 1)) {
        //                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
        //            } else if (i < pindex) {
        //                i = pindex - 4;
        //                page_html += "<li data=''><a href='#'>...</a></li>";
        //            } else {
        //                i = ptotal - 3;
        //                page_html += "<li data=''><a href='#'>...</a></li>";
        //            }
        //        }
        //    }
        //} else {
        //    for (var i = 0; i < ptotal; i++) {
        //        page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
        //    }
        //}
        //page_html += " <li data='Next'><a href='#'>Next</a></li></ul>";
        //$("#pagination").html(page_html);
        //$("#pagination li").off('click');
        //$("#pagination li").on('click', function () {
        //    var c_value = $(this).attr("data");
        //    if (c_value == "Prev") {
        //        pindex = (pindex - 1);
        //    } else if (c_value == "Next") {
        //        pindex = (pindex + 1);
        //    } else if (c_value == null || c_value.length <= 0) {
        //        return;
        //    } else {
        //        pindex = c_value;
        //    }
        //    callback(pindex, psize);
        //});
    }


    function setindex(index, size, total, callback) {
        if (index > 0) {
            pindex = index;
        }
        if (size > 0) {
            psize = size;
        }
        //if (total) {
        //    pmaxindex = Math.ceil(total / psize);
        //    ptotal = total;
        //}
        ptotal = Math.ceil(total / psize);
        var page_html = "<ul>";
        if (pindex > 1) {
            page_html += "<li data='Prev'><a href='#'>Prev</a></li>";
        }
        if (ptotal > 10) {
            if (pindex < 5) {
                for (var i = 0; i < ptotal; i++) {
                    if ((i < 6 && i != ((parseInt(pindex) - 1))) || i >= ptotal - 2) {
                        page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
                    } else if (i == (pindex - 1)) {
                        page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
                    } else {
                        i = ptotal - 3;
                        page_html += "<li data=''><a href='#'>...</a></li>";
                    }
                }
            } else if (pindex >= 5 && pindex <= 6) {
                for (var i = 0; i < ptotal; i++) {
                    if ((i < (parseInt(pindex) + 2) && i != (parseInt(pindex) - 1)) || i >= ptotal - 2) {
                        page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
                    } else if (i == (pindex - 1)) {
                        page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
                    } else {
                        i = ptotal - 3;
                        page_html += "<li data=''><a href='#'>...</a></li>";
                    }
                }
            } else if (pindex >= ptotal - 5) {
                for (var i = 0; i < ptotal; i++) {
                    if (i < 2 || (i > (pindex - 3) && i != (pindex - 1))) {
                        page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
                    } else if (i == (pindex - 1)) {
                        page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
                    } else {
                        i = pindex - 4;
                        page_html += "<li data=''><a href='#'>...</a></li>";
                    }
                }
            } else {
                for (var i = 0; i < ptotal; i++) {
                    if (i < 2 || i > ptotal - 2) {
                        page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
                    } else if (i > pindex - 3 && i < (parseInt(pindex) + 2) && i != (pindex - 1)) {
                        page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
                    } else if (i == (pindex - 1)) {
                        page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
                    } else if (i < pindex) {
                        i = pindex - 4;
                        page_html += "<li data=''><a href='#'>...</a></li>";
                    } else {
                        i = ptotal - 3;
                        page_html += "<li data=''><a href='#'>...</a></li>";
                    }
                }
            }
        } else {
            for (var i = 0; i < ptotal; i++) {
                page_html += "<li data='" + (i + 1) + "'><a href='#'>" + (i + 1) + "</a></li>";
            }
        } if (pindex < ptotal) {
            page_html += "<li data='Next'><a href='#'>Next</a></li>";
        }
        page_html += " </ul>";
        //alert(page_html);
        $("#pagination").html(page_html);
        $("#pagination li").off('click');
        $("#pagination li").on('click', function () {
            var c_value = $(this).attr("data");
            if (c_value == "Prev") {
                if (pindex > 1) {
                    pindex = (pindex - 1);
                } else {
                    return;
                }
            } else if (c_value == "Next") {
                if (pindex < ptotal - 1) {
                    pindex = (pindex + 1);
                } else {
                    return;
                }
            } else if (c_value == null || c_value.length <= 0) {
                return;
            } else {
                pindex = c_value;
            }
            callback(pindex, psize);
        });
    }

    return {
        paging: paging,
        setindex: setindex
    }
})