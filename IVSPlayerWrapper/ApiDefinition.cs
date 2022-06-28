using System;
using Foundation;
using UIKit;

namespace IVSPlayer
{

	// @interface IVSPlayerWrapper : NSObject
	[BaseType(typeof(NSObject), Name = "_TtC16IVSPlayerWrapper16IVSPlayerWrapper")]
	[DisableDefaultCtor]
	interface IVSPlayerWrapper
	{
		// -(instancetype _Nonnull)initWithCallback:(id<IVSPlayerWrapperCallback> _Nonnull)callback __attribute__((objc_designated_initializer));
		[Export("initWithCallback:")]
		[DesignatedInitializer]
		IntPtr Constructor(IIVSPlayerWrapperCallback callback);

		// -(UIView * _Nonnull)getView __attribute__((warn_unused_result("")));
		[Export("getView")]
		//[Verify(MethodToProperty)]
		UIView View { get; }

		// -(double)getLiveLatency __attribute__((warn_unused_result("")));
		[Export("getLiveLatency")]
		//[Verify(MethodToProperty)]
		double LiveLatency { get; }

		// -(void)play;
		[Export("play")]
		void Play();

		// -(void)loadWithPathStr:(NSString * _Nonnull)pathStr;
		[Export("loadWithPathStr:")]
		void LoadWithPathStr(string pathStr);
	}

	interface IIVSPlayerWrapperCallback { }

	// @protocol IVSPlayerWrapperCallback
	/*
	  Check whether adding [Model] to this declaration is appropriate.
	  [Model] is used to generate a C# class that implements this protocol,
	  and might be useful for protocols that consumers are supposed to implement,
	  since consumers can subclass the generated class instead of implementing
	  the generated interface. If consumers are not supposed to implement this
	  protocol, then [Model] is redundant and will generate code that will never
	  be used.
	*/
	[Protocol(Name = "_TtP16IVSPlayerWrapper24IVSPlayerWrapperCallback_")]
	interface IVSPlayerWrapperCallback
	{
		// @required -(void)didChangeStateWithState:(NSInteger)state;
		[Abstract]
		[Export("didChangeStateWithState:")]
		void DidChangeStateWithState(nint state);

		// @optional -(void)didFailWithErrorWithError:(NSError * _Nonnull)error;
		[Export("didFailWithErrorWithError:")]
		void DidFailWithErrorWithError(NSError error);

		// @optional -(void)willRebuffer;
		[Export("willRebuffer")]
		void WillRebuffer();
	}
}