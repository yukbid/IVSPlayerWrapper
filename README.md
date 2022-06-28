# IVSPlayerWrapper

AWS IVS Player wrapper library for Xamarin mobile apps focusing on livestream playback. I mainly just implement the important player states delegate, loading a stream, and tracking the latency.

## iOS

- The library is in `IVSPlayerRapper-iOS`

- The `.framework` files in `native-libs` are important because the I have to remove the sim-arm64 architecture from `AmazonIVSPlayer.framework` (thus this app won't run on M1 Macs).

- Using it is as simple as add the Project Reference, implement the `IIVSPlayerWrapperCallback` interface, and load the view;
  ~~~c#
  player = new IVSPlayerWrapper(this);
  player.View.Frame = View.Frame;
  
  View.AddSubview(player.View);
  player.LoadWithPathStr("https://fcc3ddae59ed.us-west-2.playback.live-video.net/api/video/v1/us-west-2.893648527354.channel.YtnrVcQbttF0.m3u8");
  ~~~

- Callback interfaces: only `DidChangeStateWithState(nint state)` is required on iOS, I've set `didFailWithErrorWithError(NSError err)` and `willRebuffer()` as optionals. And did not implement the rests.

- Refer to AWS' docs for the [PlayerState](https://aws.github.io/amazon-ivs-player-docs/1.8.2/ios/Enums/IVSPlayerState.html) integer values.

## Android

- The library is in `IVSPlayerRapper-Android`

- You'll need to add the native library from Amazon to _your app_'s `.csproj`
  ~~~xml
  <ItemGroup>
      <AndroidAarLibrary Include="../native-libs/ivs-player-1.10.0.aar" />
  </ItemGroup>
  ~~~

- Using it is similar with iOS, add Project Reference, Implement `IVSPlayerWrapperCallback` interface, and load the view;
  ~~~c#
  player = new IVSPlayerWrapper(this, this);
  player.OnCreate(savedInstanceState);
  
  player.Load("https://fcc3ddae59ed.us-west-2.playback.live-video.net/api/video/v1/us-west-2.893648527354.channel.YtnrVcQbttF0.m3u8");
  
  RelativeLayout relativeLayout = new RelativeLayout(this);
  relativeLayout.AddView(player.View);
  ~~~

- All callbacks need to be implemented (apparently no optional interface on Java), but can be left blank.

- Refer to AWS' docs for the [PlayerState](https://aws.github.io/amazon-ivs-player-docs/1.10.0/android/reference/com/amazonaws/ivs/player/Player.State.html) as string.

## Extras

### Links

- https://docs.aws.amazon.com/ivs/latest/userguide/player.html
- https://aws.github.io/amazon-ivs-player-docs/1.8.2/ios/
- https://aws.github.io/amazon-ivs-player-docs/1.10.0/android/reference/com/amazonaws/ivs/player/package-summary.html

### Xcode build scripts

~~~shell
# xcode build
xcodebuild -sdk iphoneos -arch arm64
xcodebuild -sdk iphonesimulator -arch x86_64

# merge simulator and phone architecture into a single framework
lipo -create build/Release-iphoneos/IVSPlayerWrapper.framework/IVSPlayerWrapper build/Release-iphonesimulator/IVSPlayerWrapper.framework/IVSPlayerWrapper -output build/Release-iphoneos/IVSPlayerWrapper.framework/IVSPlayerWrapper

# confirm the merge
lipo -info build/Release-iphoneos/IVSPlayerWrapper.framework/IVSPlayerWrapper

# bind the library
sharpie bind -framework build/Release-iphoneos/PlayerWrapper.framework -sdk iphoneos15.5

# remove simulator-arm64 from Amazon's framework
lipo -remove arm64 build/Release-iphonesimulator/AmazonIVSPlayer.framework/AmazonIVSPlayer -o build/Release-iphonesimulator/AmazonIVSPlayer.framework/AmazonIVSPlayer 

# merge simulator x64 and phone arm64
lipo -create build/Release-iphoneos/AmazonIVSPlayer.framework/AmazonIVSPlayer build/Release-iphonesimulator/AmazonIVSPlayer.framework/AmazonIVSPlayer -o build/Release-iphoneos/AmazonIVSPlayer.framework/AmazonIVSPlayer

lipo -info build/Release-iphonesimulator/AmazonIVSPlayer.framework/AmazonIVSPlayer

~~~

