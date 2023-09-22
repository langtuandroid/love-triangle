//
//  unity_facade.h
//  Defines API (methods) for unity apps
//
//  LICENSE_CODE ZON

#import <Foundation/Foundation.h>
#import <brdsdk/brdsdk-Swift.h>
NS_ASSUME_NONNULL_BEGIN
@interface brdsdk_facade : NSObject
@end
NS_ASSUME_NONNULL_END

#ifdef __cplusplus
extern "C" {
#endif
typedef void (*external_callback_function)(int choice);

struct __ConsentImageMeta;
struct __ConsentActionMeta;

void brdsdk_set_delegate(external_callback_function callback);
void brdsdk_init(char * _Nullable benefit,
                 int benefit_len,
                 char * _Nullable agree_btn,
                 int agree_btn_len,
                 char * _Nullable disagree_btn,
                 int disagree_btn_len,
                 const unichar *_Nullable opt_out_instructions,
                 bool skip_consent,
                 char * _Nullable language,
                 int language_len,
                 int text_color,
                 int background_color,
                 int button_color,
                 struct __ConsentImageMeta *_Nullable background_image,
                 struct __ConsentActionMeta *_Nullable opt_in_info,
                 struct __ConsentActionMeta *_Nullable opt_out_info
                 );
bool brdsdk_silent_opt_in(void);
void brdsdk_opt_out(void);
bool brdsdk_show_consent(char * _Nullable benefit,
                         int benefit_len,
                         char * _Nullable agree_btn,
                         int agree_btn_len,
                         char * _Nullable disagree_btn,
                         int disagree_btn_len,
                         char * _Nullable language);
int brdsdk_get_choice(void);
bool brdsdk_consent_shown(void);
const char * _Nullable brdsdk_get_uuid(void);
BrightAPIAuthorizationStatus brdsdk_authorize_device(void);
#ifdef __cplusplus
}
#endif
