// Code goes here

$(document).ready(function (e) {



    $("#showLoading").addClass("fa-spin");
    $('#jstree')
        .on('changed.jstree', function (e, data) {
            var i, j, r = [];
            for (i = 0, j = data.selected.length; i < j; i++) {
                r.push(data.instance.get_node(data.selected[i].text));
            }
            $('#node').html('Selected: ' + JSON.stringify(data.node.data, null, 4));
        })
        .jstree({
            'core': {
                'multiple': false,
                'data': function (node, cb) {
                    $.ajax({
                        "url": node.id === "#" ?"http://jsonplaceholder.typicode.com/users" :node.id.indexOf("user") > -1 ?
                                "http://jsonplaceholder.typicode.com/posts?userId=" + node.data.id :
                                "http://jsonplaceholder.typicode.com/comments?postId=" + node.data.id,

                        "success": function (datas) {
                            debugger;
                            var jsonArr = [];
                            if (node.id === "#") {
                                $.each(datas, function (index, user) {
                                    var obj = {};
                                    obj.id = "user" + user.id;
                                    obj.text = user.name;
                                    obj.parent = "#";
                                    obj.data = user;
                                    obj.icon = "fa fa-user";
                                    obj.children = true;
                                    jsonArr.push(obj);
                                });
                            } else if (node.id.indexOf("user") > -1) {
                                $.each(datas, function (index, post) {
                                    var obj = {};
                                    obj.id = "post" + post.id;
                                    obj.text = post.title;
                                    obj.parent = "user" + post.userId;
                                    obj.icon = "fa fa-file";
                                    obj.data = post;
                                    obj.children = true;
                                    jsonArr.push(obj);
                                });
                            } else {
                                $.each(datas, function (index, comment) {
                                    var obj = {};
                                    obj.id = "comment" + comment.id;
                                    obj.text = comment.name;
                                    obj.parent = "post" + comment.postId;
                                    obj.icon = "fa fa-comment";
                                    obj.data = comment;
                                    jsonArr.push(obj);
                                });
                            }
                            cb(jsonArr);
                        }
                    });
                }
            }
        });

});