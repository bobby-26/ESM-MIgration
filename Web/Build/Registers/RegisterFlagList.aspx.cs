using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class RegisterFlagList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuRankList.AccessRights = this.ViewState;
        MenuRankList.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            if (Request.QueryString["DocumentCourseId"] != null && Request.QueryString["type"].ToUpper().Equals("COURSE"))
            {
                ViewState["DOCUMENTCOURSEID"] = Request.QueryString["DocumentCourseId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            if (Request.QueryString["DocumentLicenseId"] != null && Request.QueryString["type"].ToUpper().Equals("LICENSE"))
            {
                ViewState["DOCUMENTLICENSEID"] = Request.QueryString["DocumentLicenseId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            if (Request.QueryString["DocumentMedicalId"] != null && Request.QueryString["type"].ToUpper().Equals("MEDICAL"))
            {
                ViewState["DOCUMENTMEDICALID"] = Request.QueryString["DocumentMedicalId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            if (Request.QueryString["OtherDocumentId"] != null && Request.QueryString["type"].ToUpper().Equals("OTHER"))
            {
                ViewState["DOCUMENTOTHERID"] = Request.QueryString["OtherDocumentId"].ToString();
                ViewState["TYPE"] = Request.QueryString["type"];

            }
            BindFlagList();
            BindMapping();
        }

    }
    private void BindFlagList()
    {
        ddlflag.DataSource = PhoenixRegistersFlag.ListFlag(null);
        ddlflag.DataBindings.DataTextField = "FLDFLAGNAME";
        ddlflag.DataBindings.DataValueField = "FLDCOUNTRYCODE";
        ddlflag.DataBind();

    }
    private void BindMapping()
    {
        BindFlagList();
        if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
        {
            if (General.GetNullableInteger(ViewState["DOCUMENTCOURSEID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    General.RadBindCheckBoxList(ddlflag, ds.Tables[0].Rows[0]["FLDFLAG"].ToString());
                }
            }
        }
        else if (ViewState["TYPE"].ToString().ToUpper().Equals("LICENSE"))
        {
            if (General.GetNullableInteger(ViewState["DOCUMENTLICENSEID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(Int16.Parse(ViewState["DOCUMENTLICENSEID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    General.RadBindCheckBoxList(ddlflag, ds.Tables[0].Rows[0]["FLDFLAG"].ToString());
                }
            }
        }
        else if (ViewState["TYPE"].ToString().ToUpper().Equals("MEDICAL"))
        {
            if (General.GetNullableInteger(ViewState["DOCUMENTMEDICALID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersDocumentMedical.EditDocumentMedical(Int16.Parse(ViewState["DOCUMENTMEDICALID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    General.RadBindCheckBoxList(ddlflag, ds.Tables[0].Rows[0]["FLDFLAG"].ToString());
                }
            }
        }
        else if (ViewState["TYPE"].ToString().ToUpper().Equals("OTHER"))
        {
            if (General.GetNullableInteger(ViewState["DOCUMENTOTHERID"].ToString()) != null)
            {
                DataSet ds = PhoenixRegistersDocumentOther.EditDocumentOther(Int16.Parse(ViewState["DOCUMENTOTHERID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    General.RadBindCheckBoxList(ddlflag, ds.Tables[0].Rows[0]["FLDFLAG"].ToString());
                }
            }
        }

    }
    protected void MenuRankList_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["TYPE"].ToString().ToUpper().Equals("COURSE"))
            {
                string FlagList = "";
                foreach (ButtonListItem li in ddlflag.Items)
                {
                    if (li.Selected)
                    {
                        FlagList += li.Value + ",";
                    }
                }

                if (FlagList != "")
                {
                    FlagList = "," + FlagList;
                }

                PhoenixRegisterCrewList.UpdateDocumentCourseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Int16.Parse(ViewState["DOCUMENTCOURSEID"].ToString()), null, null, null, FlagList, null);

            }
            else if (ViewState["TYPE"].ToString().ToUpper().Equals("LICENSE"))
            {
                string FlagList = "";
                foreach (ButtonListItem li in ddlflag.Items)
                {
                    if (li.Selected)
                    {
                        FlagList += li.Value + ",";
                    }
                }

                if (FlagList != "")
                {
                    FlagList = "," + FlagList;
                }
                PhoenixRegisterCrewList.UpdateDocumentLicenseList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Int16.Parse(ViewState["DOCUMENTLICENSEID"].ToString()), null, null,null, FlagList, null);

            }
            else if (ViewState["TYPE"].ToString().ToUpper().Equals("MEDICAL"))
            {
                string FlagList = "";
                foreach (ButtonListItem li in ddlflag.Items)
                {
                    if (li.Selected)
                    {
                        FlagList += li.Value + ",";
                    }
                }

                if (FlagList != "")
                {
                    FlagList = "," + FlagList;
                }
                PhoenixRegisterCrewList.UpdateDocumentMedicalList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Int16.Parse(ViewState["DOCUMENTMEDICALID"].ToString()), null, null,null, FlagList,  null);

            }
            else if (ViewState["TYPE"].ToString().ToUpper().Equals("OTHER"))
            {
                string FlagList = "";
                foreach (ButtonListItem li in ddlflag.Items)
                {
                    if (li.Selected)
                    {
                        FlagList += li.Value + ",";
                    }
                }

                if (FlagList != "")
                {
                    FlagList = "," + FlagList;
                }
                PhoenixRegisterCrewList.UpdateDocumentOtherdocumentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Int16.Parse(ViewState["DOCUMENTOTHERID"].ToString()), null, null,null, FlagList,  null);

            }
            BindMapping();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp2', 'codehelp1');", true);

        }
    }
    }