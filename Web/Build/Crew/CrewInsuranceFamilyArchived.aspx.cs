using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewInsuranceFamilyArchived : PhoenixBasePage
{
    string strEmployeeId = string.Empty;
    string familyid = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Insurer", "IN");
            toolbar.AddButton("Family Member", "FM");              
            CrewInsuranceFamilyArchive.AccessRights = this.ViewState;
            CrewInsuranceFamilyArchive.MenuList = toolbar.Show();
            CrewInsuranceFamilyArchive.SelectedMenuIndex = 1;

            strEmployeeId = Request.QueryString["empid"];
            ViewState["EMPLOYEEID"] = Int32.Parse(strEmployeeId);
            familyid = Request.QueryString["familyid"];
            
            if (!IsPostBack)
            {
              
            }
            BindFamilyArchiveData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewInsuranceFamilyArchive_TabStripCommand(object sender, EventArgs e)
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
                Response.Redirect("CrewInsuranceFamilyArchived.aspx?empid=" + strEmployeeId, false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindFamilyArchiveData()
    {
        try
        {

            DataSet ds = PhoenixCrewInsurance.ListCrewInsurenceFamily(Int32.Parse(strEmployeeId), 0);

            gvCrewInsuranceFamilyArchive.DataSource = ds;
            gvCrewInsuranceFamilyArchive.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewInsuranceFamilyArchive_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindFamilyArchiveData();
    }


    protected void gvCrewInsuranceFamilyArchive_ItemDataBound(object sender, GridItemEventArgs e)
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
    
    protected void gvCrewInsuranceFamilyArchive_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string lblCoverId = ((RadLabel)e.Item.FindControl("lblCoverID")).Text;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
                PhoenixCrewInsurance.ArchiveCrewFamily(int.Parse(lblCoverId), 1);
             
                BindFamilyArchiveData();
                gvCrewInsuranceFamilyArchive.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCrewInsuranceFamilyArchive_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {           
            string coverid = ((RadLabel)e.Item.FindControl("lblCoverId")).Text;

            PhoenixCrewInsurance.DeleteCrewInsuranceCover(Convert.ToInt32(coverid));
            BindFamilyArchiveData();
            gvCrewInsuranceFamilyArchive.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
}