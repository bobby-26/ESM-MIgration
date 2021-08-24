using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class OptionsAccessList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["usercode"] != null)
            {
                ViewState["USERCODE"] = Request.QueryString["usercode"].ToString();
            }

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvAccessList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string usercode = ViewState["USERCODE"] == null ? "" : ViewState["USERCODE"].ToString();
        DataSet ds = SessionUtil.AccessList(General.GetNullableInteger(usercode),
           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvAccessList.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        gvAccessList.DataSource = ds;
        gvAccessList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvAccessList.SelectedIndexes.Clear();
        gvAccessList.EditIndexes.Clear();
        gvAccessList.DataSource = null;
        gvAccessList.Rebind();
    }

    private void InsertAccessList(string name, string purpose, string departmentid)
    {
        if (!IsValidAccessList(name, purpose, departmentid))
        {
            ucError.Visible = true;
            return;
        }
        SessionUtil.InsertAccessList(name, purpose, int.Parse(departmentid));
    }

    private void UpdateAccessList(int accessid, string name, string purpose, string departmentid)
    {
        if (!IsValidAccessList(name, purpose, departmentid))
        {
            ucError.Visible = true;
            return;
        }
        SessionUtil.UpdateAccessList(accessid, name, purpose, int.Parse(departmentid));
    }

    //private void DeleteAccessList(int accessid)
    //{
       
    //}

    private void CopyAccessList(int accessid)
    {
        SessionUtil.CopyAccessList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, accessid);
    }

    private bool IsValidAccessList(string name, string purpose, string departmentid)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        int result;

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (purpose.Trim().Equals(""))
            ucError.ErrorMessage = "Purpose is required.";

        if (departmentid == null || !Int32.TryParse(departmentid, out result))
            ucError.ErrorMessage = "Department  is required";

        return (!ucError.IsError);
    }

    protected void gvAccessList_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertAccessList(
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtPurposeAdd")).Text,
                    ((UserControlDepartment)e.Item.FindControl("ucDepartmentAdd")).SelectedDepartment
                );
                Rebind();

                ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
                ((UserControlDepartment)e.Item.FindControl("ucDepartmentAdd")).DataBind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateAccessList(
                    Int16.Parse(((RadLabel)e.Item.FindControl("lblAccessID")).Text),
                    ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtPurposeEdit")).Text,
                    ((UserControlDepartment)e.Item.FindControl("ucDepartmentEdit")).SelectedDepartment
                 );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                CopyAccessList(
                    Int16.Parse(((RadLabel)e.Item.FindControl("lblAccessID")).Text)
                    );
            }
            //else if (e.CommandName.ToUpper().Equals("DELETE"))
            //{
            //    DeleteAccessList(Int32.Parse(((RadLabel)e.Item.FindControl("lblAccessID")).Text));
            //    Rebind();
            //}
            else if (e.CommandName.ToUpper().Equals("ACCESSLIST"))
            {
                string accessid = ((RadLabel)e.Item.FindControl("lblAccessID")).Text;

                if (ViewState["USERCODE"] != null)
                    Response.Redirect("OptionsUserAccess.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + accessid, false);
                else
                    Response.Redirect("OptionsUserAccess.aspx?accessid=" + accessid, false);
            }

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAccessList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            //if (db != null)
            //{
            //    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            //    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            //}

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
        if (e.Item.IsInEditMode)
        {
            
            RadTextBox Name = (RadTextBox)e.Item.FindControl("txtNameEdit");
            RadTextBox Purpose = (RadTextBox)e.Item.FindControl("txtPurposeEdit");
            UserControlDepartment ddlDepartment = (UserControlDepartment)e.Item.FindControl("ucDepartmentEdit");
            if (Name != null) Name.Text = DataBinder.Eval(e.Item.DataItem, "FLDACCESSID").ToString();
            if (Purpose != null) Purpose.Text = DataBinder.Eval(e.Item.DataItem, "FLDPURPOSE").ToString();
            if (ddlDepartment != null) ddlDepartment.SelectedDepartment = DataBinder.Eval(e.Item.DataItem, "FLDACCESSTYPE").ToString();
        }

        if (e.Item is GridDataItem)
        {
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }

    protected void gvAccessList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAccessList.CurrentPageIndex + 1;
        BindData();
    }
}
