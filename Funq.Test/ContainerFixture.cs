using NUnit.Framework;
using System;
using Funq;

namespace Funq.Test
{
    [TestFixture]
    public class ContainerFixture
    {
        [Test]
        public void ShouldRegister()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo());

            var foo = container.Resolve<IFoo>();
        }

        [Test]
        public void RegisteredInstanceIsResolved()
        {
            var container = new Container();

            var f1 = new Foo();
            container.Register<IFoo>(f1);

            var f2 = container.Resolve<IFoo>();

            Assert.AreEqual(f1, f2);
        }

        [Test]
        public void ThrowsIfCannotResolve()
        {
            var container = new Container();

            try
            {
                var foo = container.Resolve<IFoo>();
                Assert.Fail("Should have thrown ResolutionException");
            }
            catch (ResolutionException)
            {
            }
        }

        [Test]
        public void ThrowsIfCannotResolveNamed()
        {
            var container = new Container();

            try
            {
                var foo = container.ResolveNamed<IFoo>("foo");
                Assert.Fail("Should have thrown ResolutionException");
            }
            catch (ResolutionException re)
            {
                Assert.IsTrue(re.Message.IndexOf("foo") != -1);
            }
        }

        [Test]
        public void RegistersDelegateForType()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo());

            var foo = container.Resolve<IFoo>();

            Assert.IsNotNull(foo);
            Assert.IsTrue(foo is IFoo);
            Assert.IsTrue(foo is Foo);
        }

        [Test]
        public void RegistersNamedInstances()
        {
            var container = new Container();
            container.Register<IFoo>("foo", c => new Foo());
            container.Register<IFoo>("foo2", c => new Foo2());

            var foo = container.ResolveNamed<IFoo>("foo");
            var foo2 = container.ResolveNamed<IFoo>("foo2");

            Assert.AreNotSame(foo, foo2);
            Assert.IsTrue(foo is IFoo);
            Assert.IsTrue(foo2 is IFoo);
            Assert.IsTrue(foo is Foo);
            Assert.IsTrue(foo2 is Foo2);
        }

        [Test]
        public void RegistersWithCtorArguments()
        {
            var container = new Container();
            container.Register<IFoo, string>((c, s) => new Foo(s));

            var foo = container.Resolve<IFoo, string>("value");

            Assert.AreEqual("value", ((Foo)foo).Value);
        }

        [Test]
        public void RegistersWithCtorOverloads()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo());
            container.Register<IFoo, string>((c, s) => new Foo(s));
            container.Register<IFoo, string, int>((c, s, i) => new Foo(s, i));

            var foo = container.Resolve<IFoo, string>("value");
            var foo2 = container.Resolve<IFoo, string, int>("foo", 25);

            Assert.AreEqual("value", ((Foo)foo).Value);
            Assert.AreEqual("foo", ((Foo)foo2).Value);
            Assert.AreEqual(25, ((Foo)foo2).Count);
        }

        [Test]
        public void RegistersAllOverloads()
        {
            var container = new Container();

            container.Register<Bar>("bar", c => new Bar());
            container.Register<Bar, string>("bar", (c, s) => new Bar(s));
            container.Register<Bar, string, string>("bar", (c, s1, s2) => new Bar(s1, s2));
            container.Register<Bar, string, string, string>("bar", (c, s1, s2, s3) => new Bar(s1, s2, s3));
            container.Register<Bar, string, string, string, string>("bar", (c, s1, s2, s3, s4) => new Bar(s1, s2, s3, s4));
            container.Register<Bar, string, string, string, string, string>("bar", (c, s1, s2, s3, s4, s5) => new Bar(s1, s2, s3, s4, s5));
            container.Register<Bar, string, string, string, string, string, string>("bar", (c, s1, s2, s3, s4, s5, s6) => new Bar(s1, s2, s3, s4, s5, s6));

            container.Register<Bar>(c => new Bar());
            container.Register<Bar, string>((c, s) => new Bar(s));
            container.Register<Bar, string, string>((c, s1, s2) => new Bar(s1, s2));
            container.Register<Bar, string, string, string>((c, s1, s2, s3) => new Bar(s1, s2, s3));
            container.Register<Bar, string, string, string, string>((c, s1, s2, s3, s4) => new Bar(s1, s2, s3, s4));
            container.Register<Bar, string, string, string, string, string>((c, s1, s2, s3, s4, s5) => new Bar(s1, s2, s3, s4, s5));
            container.Register<Bar, string, string, string, string, string, string>((c, s1, s2, s3, s4, s5, s6) => new Bar(s1, s2, s3, s4, s5, s6));

            Assert.IsNotNull(container.Resolve<Bar>());

            var b = container.Resolve<Bar, string>("a1");
            Assert.AreEqual("a1", b.Arg1);

            b = container.Resolve<Bar, string, string>("a1", "a2");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);

            b = container.Resolve<Bar, string, string, string>("a1", "a2", "a3");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);

            b = container.Resolve<Bar, string, string, string, string>("a1", "a2", "a3", "a4");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);
            Assert.AreEqual("a4", b.Arg4);

            b = container.Resolve<Bar, string, string, string, string, string>("a1", "a2", "a3", "a4", "a5");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);
            Assert.AreEqual("a4", b.Arg4);
            Assert.AreEqual("a5", b.Arg5);

            b = container.Resolve<Bar, string, string, string, string, string, string>("a1", "a2", "a3", "a4", "a5", "a6");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);
            Assert.AreEqual("a4", b.Arg4);
            Assert.AreEqual("a5", b.Arg5);
            Assert.AreEqual("a6", b.Arg6);


            Assert.IsNotNull(container.ResolveNamed<Bar>("bar"));

            b = container.ResolveNamed<Bar, string>("bar", "a1");
            Assert.AreEqual("a1", b.Arg1);

            b = container.ResolveNamed<Bar, string, string>("bar", "a1", "a2");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);

            b = container.ResolveNamed<Bar, string, string, string>("bar", "a1", "a2", "a3");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);

            b = container.ResolveNamed<Bar, string, string, string, string>("bar", "a1", "a2", "a3", "a4");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);
            Assert.AreEqual("a4", b.Arg4);

            b = container.ResolveNamed<Bar, string, string, string, string, string>("bar", "a1", "a2", "a3", "a4", "a5");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);
            Assert.AreEqual("a4", b.Arg4);
            Assert.AreEqual("a5", b.Arg5);

            b = container.ResolveNamed<Bar, string, string, string, string, string, string>("bar", "a1", "a2", "a3", "a4", "a5", "a6");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);
            Assert.AreEqual("a4", b.Arg4);
            Assert.AreEqual("a5", b.Arg5);
            Assert.AreEqual("a6", b.Arg6);
        }

        [Test]
        public void RegisterOrderForNamedDoesNotMatter()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo());
            container.Register<IFoo>("foo", c => new Foo("foo"));

            var foo = container.Resolve<IFoo>();
            var foo2 = container.ResolveNamed<IFoo>("foo");

            Assert.IsNotNull(foo);
            Assert.IsNotNull(foo2);

            Assert.AreEqual("foo", ((Foo)foo2).Value);
        }

        [Test]
        public void TryResolveReturnsNullIfNotRegistered()
        {
            var container = new Container();

            Assert.IsNull(container.TryResolve<IFoo>());
            Assert.IsNull(container.TryResolve<IFoo, string>("a1"));
            Assert.IsNull(container.TryResolve<IFoo, string, string>("a1", "a2"));
            Assert.IsNull(container.TryResolve<IFoo, string, string, string>("a1", "a2", "a3"));
            Assert.IsNull(container.TryResolve<IFoo, string, string, string, string>("a1", "a2", "a3", "a4"));
            Assert.IsNull(container.TryResolve<IFoo, string, string, string, string, string>("a1", "a2", "a3", "a4", "a5"));
            Assert.IsNull(container.TryResolve<IFoo, string, string, string, string, string, string>("a1", "a2", "a3", "a4", "a5", "a6"));
        }

        [Test]
        public void TryResolveNamedReturnsNullIfNotRegistered()
        {
            var container = new Container();

            Assert.IsNull(container.TryResolveNamed<IFoo>("foo"));
            Assert.IsNull(container.TryResolveNamed<IFoo, string>("foo", "a1"));
            Assert.IsNull(container.TryResolveNamed<IFoo, string, string>("foo", "a1", "a2"));
            Assert.IsNull(container.TryResolveNamed<IFoo, string, string, string>("foo", "a1", "a2", "a3"));
            Assert.IsNull(container.TryResolveNamed<IFoo, string, string, string, string>("foo", "a1", "a2", "a3", "a4"));
            Assert.IsNull(container.TryResolveNamed<IFoo, string, string, string, string, string>("foo", "a1", "a2", "a3", "a4", "a5"));
            Assert.IsNull(container.TryResolveNamed<IFoo, string, string, string, string, string, string>("foo", "a1", "a2", "a3", "a4", "a5", "a6"));
        }

        [Test]
        public void TryResolveReturnsRegisteredInstance()
        {
            var container = new Container();

            container.Register<Bar>("bar", c => new Bar());
            container.Register<Bar, string>("bar", (c, s) => new Bar(s));
            container.Register<Bar, string, string>("bar", (c, s1, s2) => new Bar(s1, s2));
            container.Register<Bar, string, string, string>("bar", (c, s1, s2, s3) => new Bar(s1, s2, s3));
            container.Register<Bar, string, string, string, string>("bar", (c, s1, s2, s3, s4) => new Bar(s1, s2, s3, s4));
            container.Register<Bar, string, string, string, string, string>("bar", (c, s1, s2, s3, s4, s5) => new Bar(s1, s2, s3, s4, s5));
            container.Register<Bar, string, string, string, string, string, string>("bar", (c, s1, s2, s3, s4, s5, s6) => new Bar(s1, s2, s3, s4, s5, s6));

            Assert.IsNotNull(container.TryResolveNamed<Bar>("bar"));
            Assert.IsNotNull(container.TryResolveNamed<Bar, string>("bar", "a1"));
            Assert.IsNotNull(container.TryResolveNamed<Bar, string, string>("bar", "a1", "a2"));
            Assert.IsNotNull(container.TryResolveNamed<Bar, string, string, string>("bar", "a1", "a2", "a3"));
            Assert.IsNotNull(container.TryResolveNamed<Bar, string, string, string, string>("bar", "a1", "a2", "a3", "a4"));
            Assert.IsNotNull(container.TryResolveNamed<Bar, string, string, string, string, string>("bar", "a1", "a2", "a3", "a4", "a5"));
            Assert.IsNotNull(container.TryResolveNamed<Bar, string, string, string, string, string, string>("bar", "a1", "a2", "a3", "a4", "a5", "a6"));
        }

        [Test]
        public void TryResolveReturnsRegisteredInstanceOnParent()
        {
            var container = new Container();

            container.Register<Bar>("bar", c => new Bar());
            container.Register<Bar, string>("bar", (c, s) => new Bar(s));
            container.Register<Bar, string, string>("bar", (c, s1, s2) => new Bar(s1, s2));
            container.Register<Bar, string, string, string>("bar", (c, s1, s2, s3) => new Bar(s1, s2, s3));
            container.Register<Bar, string, string, string, string>("bar", (c, s1, s2, s3, s4) => new Bar(s1, s2, s3, s4));
            container.Register<Bar, string, string, string, string, string>("bar", (c, s1, s2, s3, s4, s5) => new Bar(s1, s2, s3, s4, s5));
            container.Register<Bar, string, string, string, string, string, string>("bar", (c, s1, s2, s3, s4, s5, s6) => new Bar(s1, s2, s3, s4, s5, s6));

            var child = container.CreateChildContainer();

            Assert.IsNotNull(child.TryResolveNamed<Bar>("bar"));
            Assert.IsNotNull(child.TryResolveNamed<Bar, string>("bar", "a1"));
            Assert.IsNotNull(child.TryResolveNamed<Bar, string, string>("bar", "a1", "a2"));
            Assert.IsNotNull(child.TryResolveNamed<Bar, string, string, string>("bar", "a1", "a2", "a3"));
            Assert.IsNotNull(child.TryResolveNamed<Bar, string, string, string, string>("bar", "a1", "a2", "a3", "a4"));
            Assert.IsNotNull(child.TryResolveNamed<Bar, string, string, string, string, string>("bar", "a1", "a2", "a3", "a4", "a5"));
            Assert.IsNotNull(child.TryResolveNamed<Bar, string, string, string, string, string, string>("bar", "a1", "a2", "a3", "a4", "a5", "a6"));
        }

        [Test]
        public void LatestRegistrationOverridesPrevious()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo());
            container.Register<IFoo>(c => new Foo("foo"));

            var foo = container.Resolve<IFoo>();

            Assert.AreEqual("foo", ((Foo)foo).Value);
        }

        [Test]
        public void DisposesContainerOwnedInstances()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Disposable()).OwnedBy(Owner.Container);
            container.Register<IBase>(c => new Disposable()).OwnedBy(Owner.External);

            var containerOwned = container.Resolve<IFoo>() as Disposable;
            var externallyOwned = container.Resolve<IBase>() as Disposable;

            container.Dispose();

            Assert.IsTrue(containerOwned.IsDisposed);
            Assert.IsFalse(externallyOwned.IsDisposed);
        }

        [Test]
        public void ChildContainerCanReuseRegistrationsOnParent()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo());

            var child = container.CreateChildContainer();

            var foo = child.Resolve<IFoo>();

            Assert.IsNotNull(foo);
        }

        [Test]
        public void NoReuseCreatesNewInstancesAlways()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo()).ReusedWithin(ReuseScope.None);

            var foo1 = container.Resolve<IFoo>();
            var foo2 = container.Resolve<IFoo>();

            Assert.IsNotNull(foo1);
            Assert.IsNotNull(foo2);
            Assert.AreNotSame(foo1, foo2);
        }

        [Test]
        public void ContainerScopedInstanceIsReused()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo()).ReusedWithin(ReuseScope.Container);


            var foo1 = container.Resolve<IFoo>();
            var foo2 = container.Resolve<IFoo>();

            Assert.IsNotNull(foo1);
            Assert.IsNotNull(foo2);
            Assert.AreSame(foo1, foo2);
        }

        [Test]
        public void HierarchyScopedInstanceIsReusedOnSameContainer()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo()).ReusedWithin(ReuseScope.Hierarchy);

            var foo1 = container.Resolve<IFoo>();
            var foo2 = container.Resolve<IFoo>();

            Assert.IsNotNull(foo1);
            Assert.IsNotNull(foo2);
            Assert.AreSame(foo1, foo2);
        }

        [Test]
        public void HierarchyScopedInstanceIsReusedFromParentContainer()
        {
            var parent = new Container();
            parent.Register<IFoo>(c => new Foo()).ReusedWithin(ReuseScope.Hierarchy);
            var child = parent.CreateChildContainer();

            var foo1 = parent.Resolve<IFoo>();
            var foo2 = child.Resolve<IFoo>();

            Assert.IsNotNull(foo1);
            Assert.IsNotNull(foo2);
            Assert.AreSame(foo1, foo2);
        }

        [Test]
        public void HierarchyScopedInstanceIsCreatedOnRegistrationContainer()
        {
            var parent = new Container();
            parent.Register<IFoo>(c => new Disposable()).ReusedWithin(ReuseScope.Hierarchy);
            var child = parent.CreateChildContainer();

            var childFoo = child.Resolve<IFoo>() as Disposable;
            var parentFoo = parent.Resolve<IFoo>() as Disposable;

            Assert.IsNotNull(parentFoo);
            Assert.IsNotNull(childFoo);

            child.Dispose();

            Assert.IsFalse(parentFoo.IsDisposed);
        }

        [Test]
        public void ContainerScopedInstanceIsNotReusedFromParentContainer()
        {
            var parent = new Container();
            parent.Register<IFoo>(c => new Foo()).ReusedWithin(ReuseScope.Container);
            var child = parent.CreateChildContainer();

            var foo1 = parent.Resolve<IFoo>();
            var foo2 = child.Resolve<IFoo>();

            Assert.IsNotNull(foo1);
            Assert.IsNotNull(foo2);
            Assert.AreNotSame(foo1, foo2);
        }

        [Test]
        public void DisposingParentContainerDisposesChildContainerAndInstances()
        {
            var parent = new Container();
            parent.Register<IFoo>(c => new Disposable()).ReusedWithin(ReuseScope.Hierarchy);
            parent.Register<IBase>(c => new Disposable()).ReusedWithin(ReuseScope.Container);
            var child = parent.CreateChildContainer();

            var parentFoo = parent.Resolve<IFoo>() as Disposable;
            var childFoo = child.Resolve<IBase>() as Disposable;

            parent.Dispose();

            Assert.IsTrue(parentFoo.IsDisposed);
            Assert.IsTrue(childFoo.IsDisposed);
        }

        [Test]
        public void ContainerOwnedNonReuseInstacesAreDisposed()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Disposable())
                .ReusedWithin(ReuseScope.None)
                .OwnedBy(Owner.Container);

            var foo = container.Resolve<IFoo>() as Disposable;

            container.Dispose();

            Assert.IsTrue(foo.IsDisposed);
        }

        [Test]
        public void ContainerOwnedNonReuseInstacesAreNotKeptAlive()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Disposable())
                .ReusedWithin(ReuseScope.None)
                .OwnedBy(Owner.Container);

            var foo = container.Resolve<IFoo>() as Disposable;
            var wr = new WeakReference(foo);

            foo = null;

            GC.Collect();

            Assert.IsFalse(wr.IsAlive);
        }

        [Test]
        public void ContainerOwnedAndContainerReusedInstacesAreDisposed()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Disposable())
                .ReusedWithin(ReuseScope.Container)
                .OwnedBy(Owner.Container);

            var foo = container.Resolve<IFoo>() as Disposable;

            container.Dispose();

            Assert.IsTrue(foo.IsDisposed);
        }

        [Test]
        public void ContainerOwnedAndHierarchyReusedInstacesAreDisposed()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Disposable())
                .ReusedWithin(ReuseScope.Hierarchy)
                .OwnedBy(Owner.Container);

            var foo = container.Resolve<IFoo>() as Disposable;

            container.Dispose();

            Assert.IsTrue(foo.IsDisposed);
        }

        [Test]
        public void ChildContainerInstanceWithParentRegistrationIsDisposed()
        {
            var parent = new Container();
            parent.Register<IFoo>(c => new Disposable())
                .ReusedWithin(ReuseScope.Hierarchy)
                .OwnedBy(Owner.Container);

            var child = parent.CreateChildContainer();

            var foo = child.Resolve<IFoo>() as Disposable;

            child.Dispose();

            Assert.IsFalse(foo.IsDisposed);
        }

        [Test]
        public void DisposingParentContainerDisposesChildContainerInstances()
        {
            var parent = new Container();
            parent.Register<IFoo>(c => new Disposable())
                .ReusedWithin(ReuseScope.None)
                .OwnedBy(Owner.Container);

            var child = parent.CreateChildContainer();

            var foo = child.Resolve<IFoo>() as Disposable;

            parent.Dispose();

            Assert.IsTrue(foo.IsDisposed);
        }

        [Test]
        public void DisposingContainerDoesNotDisposeExternalOwnedInstances()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Disposable())
                .ReusedWithin(ReuseScope.Hierarchy)
                .OwnedBy(Owner.External);

            var foo = container.Resolve<IFoo>() as Disposable;

            container.Dispose();

            Assert.IsFalse(foo.IsDisposed);
        }

        [Test]
        public void InitializerCalledWhenInstanceCreatedContainerReuse()
        {
            var container = new Container();
            container
                .Register<IInitializable>(c => new Initializable())
                .InitializedBy((c, i) => i.Initialize())
                .ReusedWithin(ReuseScope.Container);

            var i1 = container.Resolve<IInitializable>() as Initializable;
            var i2 = container.Resolve<IInitializable>() as Initializable;

            Assert.AreSame(i1, i2);
            Assert.AreEqual(1, i1.InitializeCalls);
        }

        [Test]
        public void InitializerCalledWhenInstanceCreatedHierarchyReuse()
        {
            var container = new Container();
            container
                .Register<IInitializable>(c => new Initializable())
                .InitializedBy((c, i) => i.Initialize())
                .ReusedWithin(ReuseScope.Hierarchy);

            var i1 = container.Resolve<IInitializable>() as Initializable;
            var i2 = container.Resolve<IInitializable>() as Initializable;

            Assert.AreSame(i1, i2);
            Assert.AreEqual(1, i1.InitializeCalls);
        }

        [Test]
        public void InitializerCalledWhenInstanceCreatedNoReuse()
        {
            var container = new Container();
            container
                .Register<IInitializable>(c => new Initializable())
                .InitializedBy((c, i) => i.Initialize())
                .ReusedWithin(ReuseScope.None);

            var i1 = container.Resolve<IInitializable>() as Initializable;
            var i2 = container.Resolve<IInitializable>() as Initializable;

            Assert.AreNotSame(i1, i2);
            Assert.AreEqual(1, i1.InitializeCalls);
            Assert.AreEqual(1, i2.InitializeCalls);
        }

        [Test]
        public void InitializerCalledOnChildContainerWhenInstanceCreated()
        {
            var container = new Container();
            container
                .Register<IInitializable>(c => new Initializable())
                .InitializedBy((c, i) => i.Initialize())
                .ReusedWithin(ReuseScope.Container);

            var child = container.CreateChildContainer();

            var i1 = child.Resolve<IInitializable>() as Initializable;
            var i2 = child.Resolve<IInitializable>() as Initializable;

            Assert.AreSame(i1, i2);
            Assert.AreEqual(1, i1.InitializeCalls);
        }

        [Test]
        public void InitializerCanRetrieveResolvedDependency()
        {
            var container = new Container();
            container.Register(c => new Presenter(c.Resolve<View>()));
            container.Register(c => new View())
                .InitializedBy((c, v) => v.Presenter = c.Resolve<Presenter>());

            var view = container.Resolve<View>();
            var presenter = container.Resolve<Presenter>();

            Assert.AreSame(view.Presenter, presenter);
        }

        [Test]
        public void InitializerCalledOnEntryContainer()
        {
            var container = new Container();
            // Notice the purposedful error: we register the view 
            // on the parent container, but the presenter in 
            // the child. Since the reuse is hierarchy, the view 
            // will be created on the parent, and thus the 
            // initializer should NOT be able to resolve 
            // the presenter, which lives in the child container.
            container.Register(c => new View())
                .InitializedBy((c, v) => v.Presenter = c.Resolve<Presenter>())
                .ReusedWithin(ReuseScope.Hierarchy);

            var child = container.CreateChildContainer();
            child.Register(c => new Presenter(c.Resolve<View>()));

            try
            {
                var view = child.Resolve<View>();
                Assert.Fail("Should have thrown as presenter is registered on child and initializer runs on parent");
            }
            catch (ResolutionException)
            {
            }
        }

        [Test]
        public void ThrowsIfRegisterContainerService()
        {
            try
            {
                var container = new Container();
                container.Register<Container>(c => new Container());
                Assert.Fail("Should have thrown when registering a Container service.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(FunqResources.Registration_CantRegisterContainer, ex.Message);
            }
        }

        [Test]
        public void ShouldGetContainerServiceAlways()
        {
            var container = new Container();

            var service = container.Resolve<Container>();

            Assert.IsNotNull(service);
        }

        [Test]
        public void ShouldGetSameContainerServiceAsCurrentContainer()
        {
            var container = new Container();

            var service = container.Resolve<Container>();

            Assert.AreSame(container, service);

            var child = container.CreateChildContainer();

            service = child.Resolve<Container>();

            Assert.AreSame(child, service);

            var grandChild = child.CreateChildContainer();

            service = grandChild.Resolve<Container>();

            Assert.AreSame(grandChild, service);
        }

        [Test]
        public void DefaultReuseCanBeSpecified()
        {
            var container = new Container { DefaultReuse = ReuseScope.None };

            container.Register<IFoo>(c => new Foo());

            var f1 = container.Resolve<IFoo>();
            var f2 = container.Resolve<IFoo>();

            Assert.AreNotSame(f1, f2);
        }

        [Test]
        public void DefaultOwnerCanBeSpecified()
        {
            var container = new Container { DefaultOwner = Owner.External };

            container.Register(c => new Disposable());

            var d = container.Resolve<Disposable>();

            container.Dispose();

            Assert.IsFalse(d.IsDisposed);
        }

        [Test]
        public void LazyResolveProvidedForRegisteredServices()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo()).ReusedWithin(ReuseScope.Container);

            var func = container.LazyResolve<IFoo>();

            Assert.IsNotNull(func);
        }

        [Test]
        public void LazyResolveHonorsReuseScope()
        {
            var container = new Container();
            container.Register<IFoo>(c => new Foo()).ReusedWithin(ReuseScope.Container);

            var func = container.LazyResolve<IFoo>();

            var f1 = func();
            var f2 = func();

            Assert.AreSame(f1, f2);
        }

        [Test]
        public void LazyResolve()
        {
            var container = new Container();
            container.Register("foo", c => new Foo("foo"));
            container.Register("bar", c => new Foo("bar"));

            var foo = container.LazyResolve<Foo>("foo");
            var bar = container.LazyResolve<Foo>("bar");

            Assert.IsNotNull(foo);
            Assert.IsNotNull(bar);

            Assert.AreEqual("foo", foo().Value);
            Assert.AreEqual("bar", bar().Value);
        }

        [Test]
        public void LazyResolveAllOverloads()
        {
            var container = new Container();

            container.Register<Bar>(c => new Bar());
            container.Register<Bar, string>((c, s) => new Bar(s));
            container.Register<Bar, string, string>((c, s1, s2) => new Bar(s1, s2));
            container.Register<Bar, string, string, string>((c, s1, s2, s3) => new Bar(s1, s2, s3));
            container.Register<Bar, string, string, string, string>((c, s1, s2, s3, s4) => new Bar(s1, s2, s3, s4));
            container.Register<Bar, string, string, string, string, string>((c, s1, s2, s3, s4, s5) => new Bar(s1, s2, s3, s4, s5));
            container.Register<Bar, string, string, string, string, string, string>((c, s1, s2, s3, s4, s5, s6) => new Bar(s1, s2, s3, s4, s5, s6));

            Assert.IsNotNull(container.Resolve<Bar>());

            var b = container.LazyResolve<Bar, string>()("a1");
            Assert.AreEqual("a1", b.Arg1);

            b = container.LazyResolve<Bar, string, string>()("a1", "a2");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);

            b = container.LazyResolve<Bar, string, string, string>()("a1", "a2", "a3");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);

            b = container.LazyResolve<Bar, string, string, string, string>()("a1", "a2", "a3", "a4");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);
            Assert.AreEqual("a4", b.Arg4);

            b = container.LazyResolve<Bar, string, string, string, string, string>()("a1", "a2", "a3", "a4", "a5");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);
            Assert.AreEqual("a4", b.Arg4);
            Assert.AreEqual("a5", b.Arg5);

            b = container.LazyResolve<Bar, string, string, string, string, string, string>()("a1", "a2", "a3", "a4", "a5", "a6");
            Assert.AreEqual("a1", b.Arg1);
            Assert.AreEqual("a2", b.Arg2);
            Assert.AreEqual("a3", b.Arg3);
            Assert.AreEqual("a4", b.Arg4);
            Assert.AreEqual("a5", b.Arg5);
            Assert.AreEqual("a6", b.Arg6);

        }


        [Test]
        public void LazyResolveThrowsIfNotRegistered()
        {
            var container = new Container();

            try
            {
                container.LazyResolve<IFoo>();
                Assert.Fail("Should have failed to resolve the lazy func");
            }
            catch (ResolutionException)
            {
            }

            try
            {
                container.LazyResolve<IFoo, string>();
                Assert.Fail("Should have failed to resolve the lazy func");
            }
            catch (ResolutionException)
            {
            }

            try
            {
                container.LazyResolve<IFoo, string, string>();
                Assert.Fail("Should have failed to resolve the lazy func");
            }
            catch (ResolutionException)
            {
            }

            try
            {
                container.LazyResolve<IFoo, string, string, string>();
                Assert.Fail("Should have failed to resolve the lazy func");
            }
            catch (ResolutionException)
            {
            }

            try
            {
                container.LazyResolve<IFoo, string, string, string, string>();
                Assert.Fail("Should have failed to resolve the lazy func");
            }
            catch (ResolutionException)
            {
            }

            try
            {
                container.LazyResolve<IFoo, string, string, string, string, string>();
                Assert.Fail("Should have failed to resolve the lazy func");
            }
            catch (ResolutionException)
            {
            }

            try
            {
                container.LazyResolve<IFoo, string, string, string, string, string, string>();
                Assert.Fail("Should have failed to resolve the lazy func");
            }
            catch (ResolutionException)
            {
            }

        }


        public class Presenter
        {
            public Presenter(View view)
            {
                this.View = view;
            }

            public View View { get; set; }
        }

        public class View
        {
            public Presenter Presenter { get; set; }
        }

        public class Disposable : IFoo, IDisposable
        {
            public bool IsDisposed;
            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        public interface IInitializable { void Initialize(); }
        public class Initializable : IInitializable
        {
            public int InitializeCalls;

            public void Initialize()
            {
                InitializeCalls++;
            }
        }

        public interface IBase { }
        public interface IFoo : IBase { }

        public class Foo : IFoo
        {
            public Foo() { }
            public Foo(string s) { Value = s; }
            public Foo(string s, int c) { Value = s; Count = c; }
            public string Value { get; set; }
            public int Count { get; set; }
        }

        public class Foo2 : IFoo { }

        public class Bar
        {
            public Bar()
            {
            }

            public Bar(string arg1)
            {
                this.Arg1 = arg1;
            }

            public Bar(string arg1, string arg2)
            {
                this.Arg1 = arg1;
                this.Arg2 = arg2;
            }

            public Bar(string arg1, string arg2, string arg3)
            {
                this.Arg1 = arg1;
                this.Arg2 = arg2;
                this.Arg3 = arg3;
            }

            public Bar(string arg1, string arg2, string arg3, string arg4)
            {
                this.Arg1 = arg1;
                this.Arg2 = arg2;
                this.Arg3 = arg3;
                this.Arg4 = arg4;
            }

            public Bar(string arg1, string arg2, string arg3, string arg4, string arg5)
            {
                this.Arg1 = arg1;
                this.Arg2 = arg2;
                this.Arg3 = arg3;
                this.Arg4 = arg4;
                this.Arg5 = arg5;
            }

            public Bar(string arg1, string arg2, string arg3, string arg4, string arg5, string arg6)
            {
                this.Arg1 = arg1;
                this.Arg2 = arg2;
                this.Arg3 = arg3;
                this.Arg4 = arg4;
                this.Arg5 = arg5;
                this.Arg6 = arg6;
            }

            public string Arg1 { get; set; }
            public string Arg2 { get; set; }
            public string Arg3 { get; set; }
            public string Arg4 { get; set; }
            public string Arg5 { get; set; }
            public string Arg6 { get; set; }
        }
    }}

