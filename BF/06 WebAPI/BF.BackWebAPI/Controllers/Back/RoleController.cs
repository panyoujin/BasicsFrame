using BF.BackWebAPI.Models.Back;
using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using BF.Common.StaticConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace BF.BackWebAPI.Controllers.Back
{
    public class RoleController : BaseController
    {


        /// <summary>
        /// 获取角色列表数据
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetRoleDataJson(string sort, string rolename, string roledesc, int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE, string order = "asc")
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            int startSize = 0;
            int endSize = 0;
            this.SetPageSize(page, pageSize, ref startSize, ref endSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("SortStr", sort);
            if(!string.IsNullOrWhiteSpace(rolename))
            {
                dic.Add("RoleName", string.Format("%{0}%",rolename));
            }
            if (!string.IsNullOrWhiteSpace(roledesc))
            {
                dic.Add("RoleDesc", string.Format("%{0}%", roledesc));
            }
            dic.Add("OrderStr", order);
            dic.Add("StartSize", startSize);
            dic.Add("EndSize", endSize);

            List<TB_CM_Role> data = DBBaseFactory.DALBase.QueryForList<TB_CM_Role>("get_roleList_back", dic);
            apiResult.data = new { RoleList = data, Total = data.Count };//需要修改总数
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        /// <summary>
        /// 根据角色ID 获取角色信息
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetRoleInfoByRoleID(int roleID)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("RoleID", roleID);
            //还需要加上角色对应的菜单
            apiResult.data = DBBaseFactory.DALBase.QueryForObject<TB_CM_Role>("get_roleById_back", dic);

            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }


        /// <summary>
        /// 添加角色并赋予菜单权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Create([FromBody]TB_CM_Role role)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            int roleID = role.ID;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("RoleName", role.RoleName);
            dic.Add("Description", role.Description);
            dic.Add("CreationUser", this.MemberInfo.Account);
            if (role.ID <= 0)
            {
                var objRoleID = DBBaseFactory.DALBase.ExecuteScalar("add_role_back", dic);
                int.TryParse(objRoleID + "", out roleID);
            }
            else
            {
                dic.Add("RoleID", role.ID);
                var objRoleID = DBBaseFactory.DALBase.ExecuteNonQuery("update_role_back", dic);
            }
            DBBaseFactory.DALBase.ExecuteNonQuery("delete_rolemenu_by_roleid_back", dic);
            string[] values = (role.MenuIDs == null ? null : role.MenuIDs.Split(','));
            List<int> menuidlist = new List<int>();
            if (values != null && values.Length > 0)
            {
                foreach (string value in values)
                {
                    int id;
                    if (int.TryParse(value, out id))
                    {
                        menuidlist.Add(id);
                    }
                }
                AddRoleMenu(menuidlist, roleID);
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);


        }

        private void AddRoleMenu(List<int> menuList, int id)
        {
            List<TB_CM_Rolemenu> roleMenuList = new List<TB_CM_Rolemenu>();

            foreach (var menuID in menuList)
            {
                roleMenuList.Add(new TB_CM_Rolemenu() { RoleID = id, MenuID = menuID, CreationUser = this.MemberInfo.Account, CreationDate = DateTime.Now, Status = true });

            }
            List<string> columeArr = new List<string>() { "RoleID", "MenuID", "CreationUser", "CreationDate", "Status" };
            DBBaseFactory.DALBase.BatchInsert<TB_CM_Rolemenu>("TB_CM_Rolemenu", columeArr.ToArray(), roleMenuList);

        }
        /// <summary>
        /// 获取区域树Josn
        /// </summary>
        /// <returns></returns>        
        [HttpGet]
        public HttpResponseMessage GettreeJosn(int id, bool isShowAll)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };

            List<string> menuidlist = new List<string>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("RoleID", id);
            if (id > 0)
            {
                var rolemenulist = DBBaseFactory.DALBase.QueryForList<TB_CM_Rolemenu>("get_rolemenuByRoleId_back", dic);
                if (rolemenulist != null && rolemenulist.Count > 0)
                {
                    foreach (TB_CM_Rolemenu item in rolemenulist)
                    {
                        menuidlist.Add(item.MenuID.ToString());
                    }
                }
            }
            StringBuilder parent = new StringBuilder();
            parent.Append("[");
            IList<TB_CM_Menu> menuList = DBBaseFactory.DALBase.QueryForList<TB_CM_Menu>("get_menuAllList_back", null); ;
            IList<TB_CM_Menu> allParentList = menuList.Where(p => p.IsRoot == true).ToList();
            IList<TB_CM_Menu> childmenuList = new List<TB_CM_Menu>();
            string a = string.Empty;
            if (menuList.Count > 0)
            {
                foreach (TB_CM_Menu menu in allParentList)
                {
                    GetChild(childmenuList, parent, menuList, menu, menuidlist, isShowAll);
                }
                parent.Remove(parent.Length - 1, 1);
                parent.Append("]");
            }
            apiResult.data = parent.ToString();
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        /// <summary>
        /// 递归获取树
        /// </summary>
        /// <param name="childmenuList"></param>
        /// <param name="parent"></param>
        /// <param name="menuList"></param>
        /// <param name="menu"></param>
        [Authorize]
        private void GetChild(IList<TB_CM_Menu> childmenuList, StringBuilder parent, IList<TB_CM_Menu> menuList, TB_CM_Menu menu, List<string> menulist, bool isShowAll = true)
        {

            childmenuList = menuList.Where(m => m.ParentMenuID == menu.ID).ToList();
            if (childmenuList.Count <= 0)
            {
                if (menulist.Contains(menu.ID.ToString()))
                {
                    parent.Append("{\"id\":\"" + menu.ID + "\",\"text\":\"" + menu.MenuName + "\",\"state\":\"open\",\"checked\":true");
                    parent.Append("},");
                }
                else if (isShowAll)
                {
                    parent.Append("{\"id\":\"" + menu.ID + "\",\"text\":\"" + menu.MenuName + "\",\"state\":\"open\",\"checked\":false");
                    parent.Append("},");
                }

            }
            else
            {
                if (menulist.Contains(menu.ID.ToString()) || isShowAll)
                {
                    parent.Append("{\"id\":\"" + menu.ID + "\",\"text\":\"" + menu.MenuName + "\",\"state\":\"open\"");
                    parent.Append(",\"children\":");
                    parent.Append("[");
                    foreach (TB_CM_Menu temp in childmenuList)
                    {
                        GetChild(childmenuList, parent, menuList, temp, menulist, isShowAll);
                    }
                    parent.Remove(parent.Length - 1, 1);
                    parent.Append("]");
                    parent.Append("},");
                }
            }
        }

        /// <summary>
        /// 获取该角色已授权或者未授权的用户列表
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="roleid">角色id</param>
        /// <param name="operation">判断操作，如果值为cancel，就获取该角色已授权的用户列表</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage UserDataJson(string sort, int roleid, string operation, int page = CommonConstant.PAGE, int pageSize = CommonConstant.PAGE_SIZE, string order = "asc")
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            int startSize = 0;
            int endSize = 0;
            this.SetPageSize(page, pageSize, ref startSize, ref endSize);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("SortStr", sort);
            dic.Add("RoleID", roleid);
            dic.Add("OrderStr", order);
            dic.Add("StartSize", startSize);
            dic.Add("EndSize", endSize);
            List<UserModel> data = new List<UserModel>();
            if (string.IsNullOrEmpty(operation))
            {
                //获取未授权的
                data = DBBaseFactory.DALBase.QueryForList<UserModel>("GetUnauthorizedUserByRoleid_back", dic);
            }
            else
            {
                //获取已授权的
                data = DBBaseFactory.DALBase.QueryForList<UserModel>("GetAuthorizedUserByRoleid_back", dic);
            }
            apiResult.data = new { RoleList = data, Total = data.Count };//需要修改总数
            return JsonHelper.SerializeObjectToWebApi(apiResult);

        }

        /// <summary>
        /// 角色授权
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>   
        [HttpPost]
        public HttpResponseMessage AuthorizationUser(string ids, int roleid, string operation)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            string[] values = (ids == null ? null : ids.Split(','));
            List<int> list = new List<int>();
            foreach (string value in values)
            {
                int id;
                if (int.TryParse(value, out id))
                {
                    list.Add(id);
                }
            }

            if (string.IsNullOrEmpty(operation))
            {
                List<TB_CM_UserRole> userRoleList = new List<TB_CM_UserRole>();

                foreach (var userID in list)
                {
                    userRoleList.Add(new TB_CM_UserRole() { RoleID = roleid, UserID = userID, CreationUser = this.MemberInfo.Account, CreationDate = DateTime.Now, Status = true });

                }
                List<string> columeArr = new List<string>() { "RoleID", "UserID", "CreationUser", "CreationDate", "Status" };
                DBBaseFactory.DALBase.BatchInsert<TB_CM_UserRole>("TB_CM_UserRole", columeArr.ToArray(), userRoleList);
            }
            else
            {
                //取消授权
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("UserIDs", ids);
                dic.Add("RoleID", roleid);
                dic.Add("CreationUser", this.MemberInfo.Account);
                DBBaseFactory.DALBase.ExecuteNonQuery("delete_userRole_back", dic);
            }
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }

        [HttpPost]
        public HttpResponseMessage DeleteJson(string ids)
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("RoleIDs", ids);
            //需要删除主表role，角色和用户，角色和菜单
            DBBaseFactory.DALBase.ExecuteNonQuery("delete_Role_back", dic);
            return JsonHelper.SerializeObjectToWebApi(apiResult);

        }

        public HttpResponseMessage GetMenuListByUser()
        {
            ApiResult<object> apiResult = new ApiResult<object>() { code = ResultCode.CODE_SUCCESS, msg = ResultMsg.CODE_SUCCESS };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("UserID", this.MemberInfo.ID);

            List<TB_CM_Menu> list = DBBaseFactory.DALBase.QueryForList<TB_CM_Menu>("get_menuListByUserID_back", dic) ?? new List<TB_CM_Menu>();
            List<TB_CM_Menu> parentdList = list.Where(m => m.Status == false && m.ParentMenuID == 1).OrderBy(m => m.Sort).ToList();
            MenuTreeModel model = new MenuTreeModel();
            List<MenuTreeModel> treeMenuList = new List<MenuTreeModel>();
            model.menus = treeMenuList;
            foreach (TB_CM_Menu menu in parentdList)
            {
                MenuTreeModel treeModel = new MenuTreeModel();
                treeModel.icon = menu.IConUrl;
                treeModel.menuid = menu.ID.ToString();
                treeModel.menuname = menu.MenuName;
                treeModel.url = menu.MenuUrl;
                treeMenuList.Add(treeModel);
                SetChildMenu(list, treeModel, menu, false);
            }
            apiResult.data = model;
            return JsonHelper.SerializeObjectToWebApi(apiResult);
        }
        private void SetChildMenu(List<TB_CM_Menu> alllist, MenuTreeModel model, TB_CM_Menu menu, bool isChild = true)
        {
            List<MenuTreeModel> treeMenuChildList = new List<MenuTreeModel>();
            List<TB_CM_Menu> childList = alllist.Where(m => m.Status == false && m.ParentMenuID == menu.ID).ToList();
            if (isChild)
            {
                model.child = treeMenuChildList;
            }
            else
                model.menus = treeMenuChildList;
            if (childList.Count != 0)
            {
                foreach (TB_CM_Menu menuTemp in childList)
                {
                    MenuTreeModel treeModel = new MenuTreeModel();
                    treeModel.icon = menuTemp.IConUrl;
                    treeModel.menuname = menuTemp.MenuName;
                    treeModel.menuid = menuTemp.ID.ToString();
                    treeModel.url = menuTemp.MenuUrl;
                    treeMenuChildList.Add(treeModel);
                    SetChildMenu(alllist, treeModel, menuTemp);
                }
            }
        }
    }
}
