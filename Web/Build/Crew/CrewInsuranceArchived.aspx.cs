using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewInsuranceArchived : PhoenixBasePage
{ 
    string strEmployeeId = string.Empty;    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Insurer", "IN");
            toolbar.AddButton("Family Member", "FM");                
            MenuCrewInsuranceArchive.AccessRights = this.ViewState;
            MenuCrewInsuranceArchive.MenuList = toolbar.Show();
            MenuCrewInsuranceArchive.SelectedMenuIndex = 0;

            strEmployeeId = Request.QueryString["empid"];
            ViewState["EMPLOYEEID"] = Int32.Parse(strEmployeeId);        

            if (!IsPostBack)
            {
                
            }
            BindInsuranceArchiveData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindInsuranceArchiveData()
    {
        try
        {
            DataSet ds = PhoenixCrewInsurance.ListCrewInsurance(Int32.Parse(strEmployeeId),0);            
            gvCrewInsuranceArchive.DataSource = ds;
            gvCrewInsuranceArchive.DataBind();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCrewInsuranceArchive_TabStripCommand(object sender, EventArgs e) 
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("IN"))
            {
                Response.Redirect("CrewInsuranceArchived.aspx?empid=" + strEmployeeId, false);
            }
            else if (CommandName.ToUpper().Equals("FM"))
            {
                Response.Redirect("CrewInsuranceFamilyArchived.aspx?empid=" + strEmployeeId , false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewInsuranceArchive_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindInsuranceArchiveData();
    }


    protected void gvCrewInsuranceArchive_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");


            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.INSURANCE + "&u=n'); return false;");
            }

        }
    }


    protected void gvCrewInsuranceArchive_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string employeeinsuranceid = ((RadLabel)e.Item.FindControl("lblEmployeeInsuranceId")).Text;

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
                PhoenixCrewInsurance.ArchiveCrewInsurance(int.Parse(employeeinsuranceid), 1);

                BindInsuranceArchiveData();
                gvCrewInsuranceArchive.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void gvCrewInsuranceArchive_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string employeeinsuranceid = ((RadLabel)e.Item.FindControl("lblEmployeeInsuranceId")).Text;

            PhoenixCrewInsurance.DeleteCrewInsurance(Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode),
                Convert.ToInt32(employeeinsuranceid)
                , Convert.ToInt32(ViewState["EMPLOYEEID"])
            );
            BindInsuranceArchiveData();
            gvCrewInsuranceArchive.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  


}
