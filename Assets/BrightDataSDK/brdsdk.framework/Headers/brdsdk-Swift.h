#if 0
#elif defined(__arm64__) && __arm64__
// Generated by Apple Swift version 5.7.1 (swiftlang-5.7.1.135.3 clang-1400.0.29.51)
#ifndef BRDSDK_SWIFT_H
#define BRDSDK_SWIFT_H
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wgcc-compat"

#if !defined(__has_include)
# define __has_include(x) 0
#endif
#if !defined(__has_attribute)
# define __has_attribute(x) 0
#endif
#if !defined(__has_feature)
# define __has_feature(x) 0
#endif
#if !defined(__has_warning)
# define __has_warning(x) 0
#endif

#if __has_include(<swift/objc-prologue.h>)
# include <swift/objc-prologue.h>
#endif

#pragma clang diagnostic ignored "-Wduplicate-method-match"
#pragma clang diagnostic ignored "-Wauto-import"
#if defined(__OBJC__)
#include <Foundation/Foundation.h>
#endif
#if defined(__cplusplus)
#include <cstdint>
#include <cstddef>
#include <cstdbool>
#else
#include <stdint.h>
#include <stddef.h>
#include <stdbool.h>
#endif

#if !defined(SWIFT_TYPEDEFS)
# define SWIFT_TYPEDEFS 1
# if __has_include(<uchar.h>)
#  include <uchar.h>
# elif !defined(__cplusplus)
typedef uint_least16_t char16_t;
typedef uint_least32_t char32_t;
# endif
typedef float swift_float2  __attribute__((__ext_vector_type__(2)));
typedef float swift_float3  __attribute__((__ext_vector_type__(3)));
typedef float swift_float4  __attribute__((__ext_vector_type__(4)));
typedef double swift_double2  __attribute__((__ext_vector_type__(2)));
typedef double swift_double3  __attribute__((__ext_vector_type__(3)));
typedef double swift_double4  __attribute__((__ext_vector_type__(4)));
typedef int swift_int2  __attribute__((__ext_vector_type__(2)));
typedef int swift_int3  __attribute__((__ext_vector_type__(3)));
typedef int swift_int4  __attribute__((__ext_vector_type__(4)));
typedef unsigned int swift_uint2  __attribute__((__ext_vector_type__(2)));
typedef unsigned int swift_uint3  __attribute__((__ext_vector_type__(3)));
typedef unsigned int swift_uint4  __attribute__((__ext_vector_type__(4)));
#endif

#if !defined(SWIFT_PASTE)
# define SWIFT_PASTE_HELPER(x, y) x##y
# define SWIFT_PASTE(x, y) SWIFT_PASTE_HELPER(x, y)
#endif
#if !defined(SWIFT_METATYPE)
# define SWIFT_METATYPE(X) Class
#endif
#if !defined(SWIFT_CLASS_PROPERTY)
# if __has_feature(objc_class_property)
#  define SWIFT_CLASS_PROPERTY(...) __VA_ARGS__
# else
#  define SWIFT_CLASS_PROPERTY(...)
# endif
#endif

#if __has_attribute(objc_runtime_name)
# define SWIFT_RUNTIME_NAME(X) __attribute__((objc_runtime_name(X)))
#else
# define SWIFT_RUNTIME_NAME(X)
#endif
#if __has_attribute(swift_name)
# define SWIFT_COMPILE_NAME(X) __attribute__((swift_name(X)))
#else
# define SWIFT_COMPILE_NAME(X)
#endif
#if __has_attribute(objc_method_family)
# define SWIFT_METHOD_FAMILY(X) __attribute__((objc_method_family(X)))
#else
# define SWIFT_METHOD_FAMILY(X)
#endif
#if __has_attribute(noescape)
# define SWIFT_NOESCAPE __attribute__((noescape))
#else
# define SWIFT_NOESCAPE
#endif
#if __has_attribute(ns_consumed)
# define SWIFT_RELEASES_ARGUMENT __attribute__((ns_consumed))
#else
# define SWIFT_RELEASES_ARGUMENT
#endif
#if __has_attribute(warn_unused_result)
# define SWIFT_WARN_UNUSED_RESULT __attribute__((warn_unused_result))
#else
# define SWIFT_WARN_UNUSED_RESULT
#endif
#if __has_attribute(noreturn)
# define SWIFT_NORETURN __attribute__((noreturn))
#else
# define SWIFT_NORETURN
#endif
#if !defined(SWIFT_CLASS_EXTRA)
# define SWIFT_CLASS_EXTRA
#endif
#if !defined(SWIFT_PROTOCOL_EXTRA)
# define SWIFT_PROTOCOL_EXTRA
#endif
#if !defined(SWIFT_ENUM_EXTRA)
# define SWIFT_ENUM_EXTRA
#endif
#if !defined(SWIFT_CLASS)
# if __has_attribute(objc_subclassing_restricted)
#  define SWIFT_CLASS(SWIFT_NAME) SWIFT_RUNTIME_NAME(SWIFT_NAME) __attribute__((objc_subclassing_restricted)) SWIFT_CLASS_EXTRA
#  define SWIFT_CLASS_NAMED(SWIFT_NAME) __attribute__((objc_subclassing_restricted)) SWIFT_COMPILE_NAME(SWIFT_NAME) SWIFT_CLASS_EXTRA
# else
#  define SWIFT_CLASS(SWIFT_NAME) SWIFT_RUNTIME_NAME(SWIFT_NAME) SWIFT_CLASS_EXTRA
#  define SWIFT_CLASS_NAMED(SWIFT_NAME) SWIFT_COMPILE_NAME(SWIFT_NAME) SWIFT_CLASS_EXTRA
# endif
#endif
#if !defined(SWIFT_RESILIENT_CLASS)
# if __has_attribute(objc_class_stub)
#  define SWIFT_RESILIENT_CLASS(SWIFT_NAME) SWIFT_CLASS(SWIFT_NAME) __attribute__((objc_class_stub))
#  define SWIFT_RESILIENT_CLASS_NAMED(SWIFT_NAME) __attribute__((objc_class_stub)) SWIFT_CLASS_NAMED(SWIFT_NAME)
# else
#  define SWIFT_RESILIENT_CLASS(SWIFT_NAME) SWIFT_CLASS(SWIFT_NAME)
#  define SWIFT_RESILIENT_CLASS_NAMED(SWIFT_NAME) SWIFT_CLASS_NAMED(SWIFT_NAME)
# endif
#endif

#if !defined(SWIFT_PROTOCOL)
# define SWIFT_PROTOCOL(SWIFT_NAME) SWIFT_RUNTIME_NAME(SWIFT_NAME) SWIFT_PROTOCOL_EXTRA
# define SWIFT_PROTOCOL_NAMED(SWIFT_NAME) SWIFT_COMPILE_NAME(SWIFT_NAME) SWIFT_PROTOCOL_EXTRA
#endif

#if !defined(SWIFT_EXTENSION)
# define SWIFT_EXTENSION(M) SWIFT_PASTE(M##_Swift_, __LINE__)
#endif

#if !defined(OBJC_DESIGNATED_INITIALIZER)
# if __has_attribute(objc_designated_initializer)
#  define OBJC_DESIGNATED_INITIALIZER __attribute__((objc_designated_initializer))
# else
#  define OBJC_DESIGNATED_INITIALIZER
# endif
#endif
#if !defined(SWIFT_ENUM_ATTR)
# if defined(__has_attribute) && __has_attribute(enum_extensibility)
#  define SWIFT_ENUM_ATTR(_extensibility) __attribute__((enum_extensibility(_extensibility)))
# else
#  define SWIFT_ENUM_ATTR(_extensibility)
# endif
#endif
#if !defined(SWIFT_ENUM)
# define SWIFT_ENUM(_type, _name, _extensibility) enum _name : _type _name; enum SWIFT_ENUM_ATTR(_extensibility) SWIFT_ENUM_EXTRA _name : _type
# if __has_feature(generalized_swift_name)
#  define SWIFT_ENUM_NAMED(_type, _name, SWIFT_NAME, _extensibility) enum _name : _type _name SWIFT_COMPILE_NAME(SWIFT_NAME); enum SWIFT_COMPILE_NAME(SWIFT_NAME) SWIFT_ENUM_ATTR(_extensibility) SWIFT_ENUM_EXTRA _name : _type
# else
#  define SWIFT_ENUM_NAMED(_type, _name, SWIFT_NAME, _extensibility) SWIFT_ENUM(_type, _name, _extensibility)
# endif
#endif
#if !defined(SWIFT_UNAVAILABLE)
# define SWIFT_UNAVAILABLE __attribute__((unavailable))
#endif
#if !defined(SWIFT_UNAVAILABLE_MSG)
# define SWIFT_UNAVAILABLE_MSG(msg) __attribute__((unavailable(msg)))
#endif
#if !defined(SWIFT_AVAILABILITY)
# define SWIFT_AVAILABILITY(plat, ...) __attribute__((availability(plat, __VA_ARGS__)))
#endif
#if !defined(SWIFT_WEAK_IMPORT)
# define SWIFT_WEAK_IMPORT __attribute__((weak_import))
#endif
#if !defined(SWIFT_DEPRECATED)
# define SWIFT_DEPRECATED __attribute__((deprecated))
#endif
#if !defined(SWIFT_DEPRECATED_MSG)
# define SWIFT_DEPRECATED_MSG(...) __attribute__((deprecated(__VA_ARGS__)))
#endif
#if __has_feature(attribute_diagnose_if_objc)
# define SWIFT_DEPRECATED_OBJC(Msg) __attribute__((diagnose_if(1, Msg, "warning")))
#else
# define SWIFT_DEPRECATED_OBJC(Msg) SWIFT_DEPRECATED_MSG(Msg)
#endif
#if defined(__OBJC__)
#if !defined(IBSegueAction)
# define IBSegueAction
#endif
#endif
#if !defined(SWIFT_EXTERN)
# if defined(__cplusplus)
#  define SWIFT_EXTERN extern "C"
# else
#  define SWIFT_EXTERN extern
# endif
#endif
#if !defined(SWIFT_CALL)
# define SWIFT_CALL __attribute__((swiftcall))
#endif
#if defined(__cplusplus)
#if !defined(SWIFT_NOEXCEPT)
# define SWIFT_NOEXCEPT noexcept
#endif
#else
#if !defined(SWIFT_NOEXCEPT)
# define SWIFT_NOEXCEPT 
#endif
#endif
#if defined(__cplusplus)
#if !defined(SWIFT_CXX_INT_DEFINED)
#define SWIFT_CXX_INT_DEFINED
namespace swift {
using Int = ptrdiff_t;
using UInt = size_t;
}
#endif
#endif
#if defined(__OBJC__)
#if __has_feature(modules)
#if __has_warning("-Watimport-in-framework-header")
#pragma clang diagnostic ignored "-Watimport-in-framework-header"
#endif
@import CoreFoundation;
@import ObjectiveC;
@import UIKit;
#endif

#endif
#pragma clang diagnostic ignored "-Wproperty-attribute-mismatch"
#pragma clang diagnostic ignored "-Wduplicate-method-arg"
#if __has_warning("-Wpragma-clang-attribute")
# pragma clang diagnostic ignored "-Wpragma-clang-attribute"
#endif
#pragma clang diagnostic ignored "-Wunknown-pragmas"
#pragma clang diagnostic ignored "-Wnullability"
#pragma clang diagnostic ignored "-Wdollar-in-identifier-extension"

#if __has_attribute(external_source_symbol)
# pragma push_macro("any")
# undef any
# pragma clang attribute push(__attribute__((external_source_symbol(language="Swift", defined_in="brdsdk",generated_declaration))), apply_to=any(function,enum,objc_interface,objc_category,objc_protocol))
# pragma pop_macro("any")
#endif

#if defined(__OBJC__)




typedef SWIFT_ENUM(NSInteger, Choice, open) {
  ChoiceNone = 0,
  ChoicePeer = 1,
  ChoiceNotPeer = 2,
};

@class UIColor;

/// The settings for the consent screen
SWIFT_CLASS("_TtC6brdsdk13ColorSettings")
@interface ColorSettings : NSObject
- (nonnull instancetype)initWithText_color:(UIColor * _Nullable)text_color background_color:(UIColor * _Nullable)background_color button_color:(UIColor * _Nullable)button_color OBJC_DESIGNATED_INITIALIZER;
- (nonnull instancetype)init SWIFT_UNAVAILABLE;
+ (nonnull instancetype)new SWIFT_UNAVAILABLE_MSG("-init is unavailable");
@end

@class UIImage;
@class NSString;
@class NSBundle;

/// Meta information of images for sdk consent action
SWIFT_CLASS("_TtC6brdsdk17ConsentActionInfo")
@interface ConsentActionInfo : NSObject
/// Image for button’s background. If nil button’s background color will be used
@property (nonatomic, readonly, strong) UIImage * _Nullable backgroundImage;
/// Image for button’s title. If nil button’s title will be used.
@property (nonatomic, readonly, strong) UIImage * _Nullable textImage;
@property (nonatomic, readonly, copy) NSString * _Nonnull description;
/// Creates meta structure for a consent button with created images
/// \param backgroundImage Image for button’s background. If nil button’s background color will be used
///
/// \param textImage Image for button’s title. If nil button’s title will be used.
///
- (nonnull instancetype)initWithBackgroundImage:(UIImage * _Nullable)backgroundImage textImage:(UIImage * _Nullable)textImage OBJC_DESIGNATED_INITIALIZER;
/// Creates meta structure for a consent button by using image names in bundle assets
/// \param backgroundName Name of background image in xcassets. The image is for button’s background.
/// If nil button’s background color will be used
///
/// \param textName Name of text image in xcassets. The image for button’s title. If nil button’s title will be used.
///
/// \param bundle Bundle where xsassets placed
///
- (nonnull instancetype)initWithBackgroundName:(NSString * _Nullable)backgroundName textName:(NSString * _Nullable)textName in:(NSBundle * _Nonnull)bundle OBJC_DESIGNATED_INITIALIZER;
- (nonnull instancetype)init SWIFT_UNAVAILABLE;
+ (nonnull instancetype)new SWIFT_UNAVAILABLE_MSG("-init is unavailable");
@end


/// Meta information about a consent screen background image
SWIFT_CLASS("_TtC6brdsdk22ConsentBackgroundImage")
@interface ConsentBackgroundImage : NSObject
/// Image for portrait orientation
@property (nonatomic, readonly, strong) UIImage * _Nonnull portrait;
/// Image for landscape orientation
@property (nonatomic, readonly, strong) UIImage * _Nonnull landscape;
/// Scale mode for image view
@property (nonatomic, readonly) enum UIViewContentMode scaleMode;
@property (nonatomic, readonly, copy) NSString * _Nonnull description;
/// Creates meta structure with passed params
/// \param portrait Image for portrait orientation
///
/// \param landscape Image for landscape orientation. If nil, portrait will be used
///
/// \param scaleMode Scale mode for image view. Desirable to use fit or fill values
///
- (nonnull instancetype)initWithPortrait:(UIImage * _Nonnull)portrait landscape:(UIImage * _Nullable)landscape scaleMode:(enum UIViewContentMode)scaleMode OBJC_DESIGNATED_INITIALIZER;
/// Creates meta structure by using image names in bundle assets
/// \param portraitName Name of portrait image in xcassets
///
/// \param landscapeName Name of landscape image in xcassets. If nil, portrait will be used
///
/// \param scaleMode Scale mode for image view. Desirable to use fit or fill values
///
/// \param bundle Bundle where xsassets placed
///
- (nullable instancetype)initWithPortraitName:(NSString * _Nonnull)portraitName landscapeName:(NSString * _Nullable)landscapeName scaleMode:(enum UIViewContentMode)scaleMode in:(NSBundle * _Nonnull)bundle OBJC_DESIGNATED_INITIALIZER;
- (nonnull instancetype)init SWIFT_UNAVAILABLE;
+ (nonnull instancetype)new SWIFT_UNAVAILABLE_MSG("-init is unavailable");
@end







@interface UIColor (SWIFT_EXTENSION(brdsdk))
+ (UIColor * _Nullable)fromInt:(NSInteger)hex SWIFT_WARN_UNUSED_RESULT;
/// Get UIColor from hex string, e.g. “FF0000” -> red color
+ (UIColor * _Nullable)fromString:(NSString * _Nullable)hexString SWIFT_WARN_UNUSED_RESULT;
@end








@protocol UIViewControllerTransitionCoordinator;
@class UITraitCollection;
@class NSCoder;

SWIFT_CLASS("_TtC6brdsdk20abstract_peer_dialog")
@interface abstract_peer_dialog : UIViewController
- (void)viewDidLoad;
- (void)viewWillAppear:(BOOL)animated;
- (void)viewDidAppear:(BOOL)animated;
- (void)viewWillTransitionToSize:(CGSize)size withTransitionCoordinator:(id <UIViewControllerTransitionCoordinator> _Nonnull)coordinator;
- (void)traitCollectionDidChange:(UITraitCollection * _Nullable)previousTraitCollection;
- (nonnull instancetype)initWithNibName:(NSString * _Nullable)nibNameOrNil bundle:(NSBundle * _Nullable)nibBundleOrNil OBJC_DESIGNATED_INITIALIZER;
- (nullable instancetype)initWithCoder:(NSCoder * _Nonnull)coder OBJC_DESIGNATED_INITIALIZER;
@end

@class NSURL;
enum BrightAPIAuthorizationStatus : NSInteger;

SWIFT_CLASS("_TtC6brdsdk7brd_api")
@interface brd_api : NSObject
+ (NSInteger)get_choice SWIFT_WARN_UNUSED_RESULT;
/// Retrieves current sdk uuid value.
///
/// returns:
/// Nil in case sdk is not initialized, or stored uuid value
+ (NSString * _Nullable)get_uuid SWIFT_WARN_UNUSED_RESULT;
/// Opts-in and starts sdk process if acceptable
/// This method checks authorization status to determine the possibility of the operation.
///
/// throws:
/// <a href="doc:brd_api/api_error">doc:brd_api/api_error</a>
+ (BOOL)silent_opt_inAndReturnError:(NSError * _Nullable * _Nullable)error;
+ (void)opt_out;
+ (void)clear_choice;
- (nullable instancetype)initWithBenefit:(NSString * _Nullable)benefit agree_btn:(NSString * _Nullable)agree_btn disagree_btn:(NSString * _Nullable)disagree_btn opt_out_instructions:(NSString * _Nullable)opt_out_instructions tracking_id:(NSString * _Nullable)tracking_id cwd:(NSURL * _Nullable)_cwd sys_app_id:(NSString * _Nullable)sys_app_id language:(NSString * _Nullable)language colors:(ColorSettings * _Nullable)colors background_image:(ConsentBackgroundImage * _Nullable)background_image opt_in_info:(ConsentActionInfo * _Nullable)opt_in_info opt_out_info:(ConsentActionInfo * _Nullable)opt_out_info skip_consent:(BOOL)skip_consent error:(NSError * _Nullable * _Nullable)error on_choice_change:(void (^ _Nullable)(NSInteger))on_choice_change OBJC_DESIGNATED_INITIALIZER;
/// Triggers post actions when custom consent screen was shown
/// When you implement custom consent screen you must call this method when the screen is presented
///
/// returns:
/// True if no errors, false when sdk authorization status is not <a href="doc:AuthorizationStatus/authorized">doc:AuthorizationStatus/authorized</a>
+ (BOOL)consent_shown SWIFT_WARN_UNUSED_RESULT;
/// Checks availability of running sdk on the device. It allows you to determine what you are able to do with sdk.
/// You should check the status and decide your reaction on it before showing your custom consent screen.
/// Example:
/// \code
/// let api = try brd_api(...)
/// ...
/// let authStatus = brd_api.authorizeDevice()
/// switch authStatus {
///     case .sdkNotInitialized: break // you have to initialize sdk before
///     case .parentControlEnabled: break // you cannot use sdk when parent control on a device is enabled
///     case .authorized:
///         if Feature.customConsent {
///             // show your own consent screen
///         } else {
///             fallthrough // for example, go to show sdk's consent
///         }
///     case .onlySDKConsent:
///         // show only sdk's consent screen
///         let shown = brd_api.show_consent(...)
/// }
///
/// \endcodeattention:
/// This method is required to call when you attempt to use your own consent screen. In the other case it’s optional.
///
/// returns:
/// Authorization status
+ (enum BrightAPIAuthorizationStatus)authorizeDevice SWIFT_WARN_UNUSED_RESULT;
- (nonnull instancetype)init SWIFT_UNAVAILABLE;
+ (nonnull instancetype)new SWIFT_UNAVAILABLE_MSG("-init is unavailable");
@end

/// Result of checks of availability of using SDK
typedef SWIFT_ENUM_NAMED(NSInteger, BrightAPIAuthorizationStatus, "AuthorizationStatus", open) {
/// Indicates that you are able to use SDK in both cases: either with SDK’s consent screen or with your own one
  BrightAPIAuthorizationStatusAuthorized = 0,
/// Indicates that SDK is not initialized
  BrightAPIAuthorizationStatusSdkNotInitialized = -1,
/// Indicates that parent control is enabled and you don’t have to use SDK
  BrightAPIAuthorizationStatusParentControlEnabled = -2,
/// Indicates that you can use only SDK’s consent screen, what means you don’t allowed to call <a href="doc:brd_api/silent_opt_in()">doc:brd_api/silent_opt_in()</a>
  BrightAPIAuthorizationStatusOnlySDKConsent = -3,
};


@interface brd_api (SWIFT_EXTENSION(brdsdk))
+ (BOOL)show_consent:(UIViewController * _Nullable)parent benefit:(NSString * _Nullable)benefit agree_btn:(NSString * _Nullable)agree_btn disagree_btn:(NSString * _Nullable)disagree_btn language:(NSString * _Nullable)language;
@end




SWIFT_CLASS("_TtC6brdsdk11peer_dialog")
@interface peer_dialog : abstract_peer_dialog
- (void)viewDidLoad;
- (void)viewWillAppear:(BOOL)animated;
- (nonnull instancetype)initWithNibName:(NSString * _Nullable)nibNameOrNil bundle:(NSBundle * _Nullable)nibBundleOrNil OBJC_DESIGNATED_INITIALIZER;
- (nullable instancetype)initWithCoder:(NSCoder * _Nonnull)coder OBJC_DESIGNATED_INITIALIZER;
@end

#endif
#if defined(__cplusplus)
#endif
#if __has_attribute(external_source_symbol)
# pragma clang attribute pop
#endif
#pragma clang diagnostic pop
#endif

#else
#error unsupported Swift architecture
#endif
