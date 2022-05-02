﻿using _0_Framework.Application;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ServiceHost {
    [HtmlTargetElement(Attributes = "Permission")]
    public class PermissionTagHelper: TagHelper {
        private readonly IAuthHelper _authHelper;
        public int Permission { get; set; }

        public PermissionTagHelper(IAuthHelper authHelper) {
            _authHelper = authHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (!_authHelper.IsAuthenticated()) {
                output.SuppressOutput();
                return;
            }
            var permissions = _authHelper.GetPermissions();
            if (permissions.All(x=>x!=Permission)) {
                output.SuppressOutput();
                return;
            }
            base.Process(context, output);
        }
    }
}