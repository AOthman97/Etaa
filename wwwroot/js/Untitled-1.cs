TempData["FinancialStatemntId"] = "FinancialStatemntId";
                    var RedirectURL = Url.Action(nameof(Index), ViewData["UserId"] = new SelectList(_context.IdentityUser, "UserId", "NameAr", User.GetLoggedInUserId<string>()));
                    return Json(new
                    {
                        redirectUrl = RedirectURL
                    });
                    